using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {

        internal async void GetHubsList(HubModel hubModel , CancellationToken ct)
        {
            var hubs = socketBrocker.GetHubsList();

            var sendMessage = new BaseMessaginModel()
            {
                Body = hubs,
                MessageType = MessageType.Message.GetHashCode()
            };

            var jsonData = serializationBroker.Serilize(sendMessage);
            await socketBrocker.SendStringAsync(hubModel, jsonData, ct);

        }

        internal async void GetSpcificHubMessageList(HubModel CurrentHubModel,string hubId, CancellationToken ct)
        {
            var messages = socketBrocker.GetHubsMessages(hubId);

            var sendMessage = new BaseMessaginModel()
            {
                Body = messages,
                MessageType = MessageType.Message.GetHashCode()
            };

            var jsonData = serializationBroker.Serilize(sendMessage);
            await socketBrocker.SendStringAsync(CurrentHubModel, jsonData, ct);

        }
    }
}
