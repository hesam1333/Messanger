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

            string respond = await SocketClient.ReceiveMessageAsync(ct);
            BaseMessaginModel resivedMessage = JsonConvert.DeserializeObject<BaseMessaginModel>(respond);

            if(resivedMessage.MessageType == 100)
            {
                var body = (JArray)resivedMessage.Body;

                for(int i = 1; i <= body.Count; i++)
                {
                    Console.WriteLine("User #"+ i+ " Id is : " + body[i-1]);
                }
            }
            else
            {
                var body = (string)resivedMessage.Body;
                Console.WriteLine(body);
            }
        }

    }
}

