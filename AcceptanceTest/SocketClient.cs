using System.Net.WebSockets;
using System.Text;

namespace AcceptanceTest
{
    public class SocketClient
    {
        ClientWebSocket wsClient = new ClientWebSocket();
        string server = "ws://localhost:5052/";

        public async Task OpenConnectionAsync(CancellationToken token)
        {

            //Set keep alive interval 5 min
            wsClient.Options.KeepAliveInterval = TimeSpan.FromMinutes(5);

            //Set desired headers
            wsClient.Options.SetRequestHeader("Host", "localhost:5052");

            //Add sub protocol if it's needed
            wsClient.Options.AddSubProtocol("zap-protocol-v1");


            await wsClient.ConnectAsync(new Uri(server), token).ConfigureAwait(false);
        }

        //Send message
        public async Task SendAsync(string message, CancellationToken token)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            await wsClient.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
        }

        //Receiving messages
        public async Task ReceiveMessageAsync(byte[] buffer)
        {
            while (true)
            {
                try
                {
                    var result = await wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);

                    //Here is the received message as string
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine(message);
                    if (result.EndOfMessage) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in receiving messages: {err}", ex.Message);
                    break;
                }
            }
        }

        public async Task HandleMessagesAsync(CancellationToken token)
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
