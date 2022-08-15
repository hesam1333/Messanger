using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcceptanceTest
{
    public partial class Program
    {

        private static async Task ShowAllUsers(CancellationToken ct)
        {
            var socketCommand = new BaseMessaginModel()
            {
                Body = null,
                Command = "getList",
                MessageType = 300, // command
            };

            var json = JsonConvert.SerializeObject(socketCommand);
            await SocketClient.SendAsync(json, ct);

        }

    }
}

