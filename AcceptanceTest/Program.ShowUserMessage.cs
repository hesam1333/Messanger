using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcceptanceTest
{
    public partial class Program
    {

        private static async Task ShowUserMessages(CancellationToken ct)
        {

            Console.Write("type user Id : ");
            var userId = Console.ReadLine();

            var socketCommand = new BaseMessaginModel()
            {
                Body = userId,
                Command = "getMessageList",
                MessageType = 300, // command
            };

            var json = JsonConvert.SerializeObject(socketCommand);
            await SocketClient.SendAsync(json, ct);

        }

    }
}

