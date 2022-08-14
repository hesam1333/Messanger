using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {

        internal void HandelResivedMessageAsync(BaseMessaginModel response)
        {
            if (response.MessageType != MessageType.Command.GetHashCode()) return;

            if(response.Command == "sendMsg")
            {

            }
            if(response.Command == "getList")
            {

            }
            if(response.Command == "getMessageList")
            {

            }

            if (response.Command == "quit")
            {

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
