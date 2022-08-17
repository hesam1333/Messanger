using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    enum MessageType
    {
        Message = 100,
        Command = 300,
        Error = 200,
    }

    public partial class ApplicationService
    {
        HubPool pool;
        ISocketBrocker socketBrocker;
        ISerializationBroker serializationBroker;
        public ApplicationService(HubPool pool, ISocketBrocker socketBrocker, ISerializationBroker serializationBroker)
        {
            this.pool = pool;
            this.socketBrocker = socketBrocker;
            this.serializationBroker = serializationBroker;
        }

    }
}
