using Messenger.Brockers;
using Messenger.Domain;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {

        internal async Task HandelResivedMessageAsync(
            HubModel currentHub, string currenthubId, BaseMessaginModel response, CancellationToken ct)
        {
            if (response.MessageType != MessageType.Command.GetHashCode()) return;
            var body = response.Body;

            if (response.Command == "sendMsg")
            {
                JObject jmessage = body as JObject;
                var message = jmessage.ToObject<MessageModel>();

                await SendMessageToHub(message.MessageBody, message.ResiverId, currenthubId, ct);
            }
            if (response.Command == "getList")
            {
                await this.GetHubsList(currentHub, ct);
            }
            if (response.Command == "getMessageList")
            {
                var wantedHub = (string)body;
                await this.GetSpcificHubMessageList(currentHub, wantedHub, ct);
            }



        }

        public async Task<BaseMessaginModel> ListenToHubAsync(HubModel hub, CancellationToken ct)
        {
            var message = await socketBrocker.ReceiveStringAsync(hub, ct);
            BaseMessaginModel? resivedMessage;
            try
            {
                resivedMessage = serializationBroker.Deserilize<BaseMessaginModel>(message);
            }
            catch
            {
                throw new Exception("format is not correct");
            }

            return resivedMessage;
        }


    }
}
