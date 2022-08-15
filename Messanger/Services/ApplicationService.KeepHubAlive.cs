using Messenger.Brockers;
using Messenger.Domain;
using System.Net.WebSockets;

namespace Messenger.Services
{

    public partial class ApplicationService
    {
        public async Task KeepSocketAlive(HubModel currentHub, string hubId, CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var response = await this.ListenToHubAsync(currentHub, ct);

                    if (response == null)
                    {
                        if (currentHub.WebSocket.State != WebSocketState.Open)
                            break;
                    }
                    else
                    {
                        await this.HandelResivedMessageAsync(currentHub, hubId, response, ct);
                    }
                }
                catch (Exception e)
                {
                    await this.SendNotifToHub(hubId, e.Message, ct);
                }
            }
        }
    }
}
