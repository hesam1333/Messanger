using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {
        public async Task SendMessageToHub(string message, string TohubId, string senderId, CancellationToken ct = default)
        {
            HubModel hub;
            if (!pool.hubs.TryGetValue(TohubId, out hub))
                throw new Exception("hub not found");

            var msgModel = new MessageModel()
            {
                CreateDateTime = DateTime.UtcNow,
                MessageBody = message,
                SenderId = senderId,
                ResiverId = TohubId
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

        public async Task SendEroreToHub(string TohubId , Exception ex , CancellationToken ct = default)
        {
            HubModel hub;
            if (!pool.hubs.TryGetValue(TohubId, out hub))
                throw new Exception("hub not found");

            var msgModel = new MessageModel()
            {
                CreateDateTime = DateTime.UtcNow,
                MessageBody = ex.Message,
                SenderId = "system",
                ResiverId = TohubId
            };

            hub.Messages.Append(msgModel);

            var sendMessage = new BaseMessaginModel()
            {
                Body = msgModel,
                MessageType = MessageType.Error.GetHashCode()
            };
            var jsonData = serializationBroker.Serilize(sendMessage);

            await socketBrocker.SendStringAsync(hub, jsonData, ct);
        }
    }
}
