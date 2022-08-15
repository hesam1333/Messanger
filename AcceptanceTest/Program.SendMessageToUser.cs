using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            string respond = await SocketClient.ReceiveMessageAsync(ct);
            BaseMessaginModel resivedMessage = JsonConvert.DeserializeObject<BaseMessaginModel>(respond);

            var jbody = (JObject)resivedMessage.Body;
            var body = jbody.ToObject<MessageModel>();

            Console.WriteLine(body.MessageBody);


        }

    }
}

