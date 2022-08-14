using Messenger.Domain;
using System.Net.WebSockets;
using System.Text;

namespace Messenger.Brockers
{
    public class SocketBrocker : ISocketBrocker
    {
        HubPool hubPool;

        public SocketBrocker(HubPool hubPool)
        {
            this.hubPool = hubPool;
        }

        public IEnumerable<string> GetHubsList(CancellationToken ct = default)
        {
           return hubPool.hubs.Select(i => i.Key);
        }

        public IEnumerable<MessageModel> GetHubsMessages(string hubId, CancellationToken ct = default)
        {
            var hub = hubPool.hubs[hubId];

            return hub.Messages;
        }

        public async ValueTask<string> ReceiveStringAsync(HubModel FromHub, CancellationToken ct = default)
        {
            var socket = FromHub.WebSocket;

            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return "";
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public Task SendStringAsync(HubModel ToHub, string data, CancellationToken ct = default)
        {
            var socket = ToHub.WebSocket;


            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
    }
}
