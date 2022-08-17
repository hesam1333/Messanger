using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcceptanceTest
{
    public partial class Program
    {
        private static async void listenToRespond(CancellationToken ct)
        {
            while (true)
            {
                string respond = await SocketClient.ReceiveMessageAsync(ct);
                BaseMessaginModel resivedMessage = JsonConvert.DeserializeObject<BaseMessaginModel>(respond);

                if (resivedMessage.MessageType == 100)
                    Console.WriteLine("\n\nRecived info::\n");
                else
                    Console.WriteLine("\n\nRecived error::\n");

                if (resivedMessage.Body is JArray)
                {
                    var Jbody = (JArray)resivedMessage.Body;
                    var body = Jbody.ToObject<List<object>>();

                    foreach (var item in body)
                    {
                        if (item is string)
                        {
                            Console.WriteLine(String.Format("user Id :" + item.ToString() + "\n\n"));
                        }
                        else
                        {
                            var jbody = (JObject)item;
                            var model = jbody.ToObject<MessageModel>();

                            Console.WriteLine(String.Format("sender Id : {0}  ", model.SenderId));
                            Console.WriteLine(String.Format("dateTime : {0}  ", model.CreateDateTime.ToString()));
                            Console.WriteLine(String.Format("message : {0}  \n\n", model.MessageBody));
                        }
                    }
                }
                else
                {
                    var jbody = (JObject)resivedMessage.Body;
                    var body = jbody.ToObject<MessageModel>();

                    Console.WriteLine(body.MessageBody + "\n\n");
                }

            }
        }
    }
}

