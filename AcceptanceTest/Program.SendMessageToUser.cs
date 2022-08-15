using Newtonsoft.Json;
namespace AcceptanceTest
{
    public partial class Program
    {
        private static async Task SendMessageToUser(CancellationToken ct)
        {
            Console.Write("type user Id : ");
            var resiverId = Console.ReadLine();


            Console.Write("type your Message : ");
            var message = Console.ReadLine();

            var socketCommand = new BaseMessaginModel()
            {
                Body = new { ResiverId = resiverId, MessageBody = message },
                Command = "sendMsg",
                MessageType = 300, // command
            };

            var json = JsonConvert.SerializeObject(socketCommand);
            await SocketClient.SendAsync(json, ct);

        }

    }
}

