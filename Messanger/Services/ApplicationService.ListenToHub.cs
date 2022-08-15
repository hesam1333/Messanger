using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {

        internal async Task HandelResivedMessageAsync(HubModel currentHub , BaseMessaginModel response , CancellationToken ct)
        {
            if (response.MessageType != MessageType.Command.GetHashCode()) return;
            var body = response.Body;

            try
            {
                if (response.Command == "sendMsg")
                {
                    var message = (MessageModel)body;
                    await SendMessageToHub(message.MessageBody, message.ResiverId, message.SenderId, ct);
                }
                if (response.Command == "getList")
                {
                    await this.GetHubsList(currentHub , ct);
                }
                if (response.Command == "getMessageList")
                {
                    var wantedHub = (string)body;
                    await this.GetSpcificHubMessageList(currentHub, wantedHub, ct);
                }

            }
            catch
            {
                throw new Exception("message is not in correct format") ;
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
