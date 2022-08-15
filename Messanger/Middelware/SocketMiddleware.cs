using Messenger.Domain;
using Messenger.Services;
using System.Net.WebSockets;

namespace Messenger.Middelware
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

            var hubModel = new HubModel() { WebSocket = currentSocket };
            var socketId = applicationService.RegisterToNetwork(hubModel);

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

            await LogOutFromNetwork(currentSocket, socketId, ct);
        }

       

    }
}
