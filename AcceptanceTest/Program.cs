
using System.Net.WebSockets;
using System.Text;

namespace AcceptanceTest
{
    public class Program
    {

        static SocketClient SocketClient = new SocketClient();

        public static  async Task Main(string[] args)
        {
            using (CancellationTokenSource cTokenSource = new CancellationTokenSource())
            {
                var ct = cTokenSource.Token;
                await SocketClient.OpenConnectionAsync(ct);

                await SocketClient.SendAsync("test", ct);

                byte[] buffer = new byte[5431];
                await SocketClient.ReceiveMessageAsync(buffer);

                Console.ReadLine();
            }
        }



    }
}

