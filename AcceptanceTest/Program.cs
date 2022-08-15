
using System.Net.WebSockets;
using System.Text;

namespace AcceptanceTest
{
    public class Program
    {
        static ClientWebSocket wsClient = new ClientWebSocket();

        public static  async Task Main(string[] args)
        {
            using (CancellationTokenSource cTokenSource = new CancellationTokenSource())
            {
                var ct = cTokenSource.Token;
                await OpenConnectionAsync(ct);

                await SendAsync("test", ct);
            }
        }


        public static async Task OpenConnectionAsync(CancellationToken token)
        {

            //Set keep alive interval
            wsClient.Options.KeepAliveInterval = TimeSpan.Zero;

            //Set desired headers
            wsClient.Options.SetRequestHeader("Host", "localhost:5052");

            //Add sub protocol if it's needed
            wsClient.Options.AddSubProtocol("zap-protocol-v1");


            await wsClient.ConnectAsync(new Uri("http://localhost:5052/"), token).ConfigureAwait(false);
        }

        //Send message
        public static async Task SendAsync(string message, CancellationToken token)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            await wsClient.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
        }

        //Receiving messages
        private static async Task ReceiveMessageAsync(byte[] buffer)
        {
            while (true)
            {
                try
                {
                    var result = await wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);

                    //Here is the received message as string
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (result.EndOfMessage) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in receiving messages: {err}", ex.Message);
                    break;
                }
            }
        }

        public static async Task HandleMessagesAsync(CancellationToken token)
        {
            var buffer = new byte[1024 * 4];
            while (wsClient.State == WebSocketState.Open)
            {
                await ReceiveMessageAsync(buffer);
            }
            if (wsClient.State != WebSocketState.Open)
            {
                Console.WriteLine("Connection closed. Status: {s}", wsClient.State.ToString());
                // Your logic if state is different than `WebSocketState.Open`
            }
        }

    }
}

