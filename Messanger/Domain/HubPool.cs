using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Messenger.Domain
{
    public class HubPool
    {
        public ConcurrentDictionary<string, HubModel> hubs { get; set; } = new ConcurrentDictionary<string, HubModel>();
    }
}
