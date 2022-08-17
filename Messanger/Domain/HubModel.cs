using System.Net.WebSockets;

namespace Messenger.Domain
{
    public class HubModel
    {
        public WebSocket WebSocket { get; set; }
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
    }
}
