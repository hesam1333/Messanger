using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {
        public async Task SendMessageToHub(string message, string TohubId, string senderId, CancellationToken ct = default)
        {
            var hub = pool.hubs[TohubId];
            if (hub == null)
                throw new Exception("hub not found");

            var msgModel = new MessageModel()
            {
                CreateDateTime = DateTime.UtcNow,
                MessageBody = message,
                SenderId = senderId,
            };

            hub.Messages.Append(msgModel);

            var sendMessage = new BaseMessaginModel()
            {
                Body = msgModel,
                MessageType = MessageType.Message.GetHashCode()
            };
            var jsonData = serializationBroker.Serilize(sendMessage);

            await socketBrocker.SendStringAsync(hub, jsonData, ct);
        }

    }
}
