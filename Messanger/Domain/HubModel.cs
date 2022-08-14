using System.Net.WebSockets;

namespace Messenger.Domain
{
    public class HubModel
    {
        public WebSocket WebSocket { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; } = Enumerable.Empty<MessageModel>();
    }
}
