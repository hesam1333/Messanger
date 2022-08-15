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

            await applicationService.KeepSocketAlive(currentHub, hubId, ct);

            await applicationService.LogOutFromNetwork(currentHub, hubId, ct);
        }

        
    }
}
