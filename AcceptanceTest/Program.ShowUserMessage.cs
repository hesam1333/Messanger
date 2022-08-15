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

            //string respond = await SocketClient.ReceiveMessageAsync(ct);
            //BaseMessaginModel resivedMessage = JsonConvert.DeserializeObject<BaseMessaginModel>(respond);

            //if (resivedMessage.MessageType == 100)
            //{
            //    var Jbody = (JArray)resivedMessage.Body;
            //    var body = Jbody.ToObject<List<MessageModel>>();

            //    Console.WriteLine(String.Format("messages list for {0} : \n", userId));
            //    foreach (var item in body)
            //    {
            //        Console.WriteLine(String.Format("sender Id : {0}  ", item.SenderId));
            //        Console.WriteLine(String.Format("dateTime : {0}  ", item.CreateDateTime.ToString()));
            //        Console.WriteLine(String.Format("message : {0}  \n\n", item.MessageBody));
            //    }
            //}
            //else
            //{
            //    var jbody = (JObject)resivedMessage.Body;
            //    var body = jbody.ToObject<MessageModel>();

            //    Console.WriteLine(body.MessageBody);
            //}
        }

    }
}

