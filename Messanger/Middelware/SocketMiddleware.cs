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

            var currentHub = new HubModel() { WebSocket = currentSocket };
            var hubId = applicationService.RegisterToNetwork(currentHub);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    var response = await applicationService.ListenToHubAsync(currentHub, ct);

                    if (response == null)
                    {
                        if (currentHub.WebSocket.State != WebSocketState.Open)
                            break;
                    }
                    else
                    {
                        await applicationService.HandelResivedMessageAsync(currentHub , hubId, response, ct);
                    }
                }
                catch (Exception e)
                {
                    await applicationService.SendEroreToHub(hubId, e, ct);
                }

            }
            await applicationService.LogOutFromNetwork(currentHub, hubId, ct);
        }



    }
}
