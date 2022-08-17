using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Brockers
{
    public interface ISocketBrocker
    {
        Task SendStringAsync(HubModel ToHub, string data, CancellationToken ct = default(CancellationToken));
        ValueTask<string> ReceiveStringAsync(HubModel FromHub, CancellationToken ct = default(CancellationToken));
        IEnumerable<string> GetHubsList();
        IEnumerable<MessageModel> GetHubsMessages(string hubId);
    }
}
