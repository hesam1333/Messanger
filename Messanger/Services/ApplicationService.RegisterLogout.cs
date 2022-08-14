using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {
        public string RegisterToNetwork(HubModel hubModel)
        {
            var socketId = Guid.NewGuid().ToString();
            pool.hubs.TryAdd(socketId, hubModel);

            return socketId;

        }

        private async Task LogOutFromNetwork(HubModel hubModel, string socketId, CancellationToken ct)
        {
            pool.hubs.TryRemove(socketId, out hubModel);

            await hubModel.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            hubModel.WebSocket.Dispose();
        }

    }
}
