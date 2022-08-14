using Messenger.Domain;
using Messenger.Services;
using System.Net.WebSockets;

namespace Messenger.Controllers
{
    public class SocketMiddleware
    {
        readonly RequestDelegate next;
        HubPool pool;
        ApplicationService applicationService;
        public SocketMiddleware(RequestDelegate next, HubPool pool, ApplicationService applicationService)
        {
            this.next = next;
            this.pool = pool;
            this.applicationService = applicationService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next.Invoke(context);
                return;
            }

            CancellationToken ct = context.RequestAborted;
            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();

            var hubModel = new HubModel() { WebSocket = currentSocket };

            pool.hubs.TryAdd(socketId, hubModel);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                var response = await applicationService.ListenToHubAsync(hubModel, ct);

                if (response == null)
                {
                    if (hubModel.WebSocket.State != WebSocketState.Open)
                        break;
                }
                else
                {
                    applicationService.HandelResivedMessageAsync(response);
                }
            }

            HubModel dummy;
            pool.hubs.TryRemove(socketId, out dummy);

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }
    }
}
