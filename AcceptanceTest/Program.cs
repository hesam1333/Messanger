using Newtonsoft.Json;
namespace AcceptanceTest
{
    public partial class Program
    {

        static SocketClient SocketClient = new SocketClient();

        public static async Task Main(string[] args)
        {
            using (CancellationTokenSource cTokenSource = new CancellationTokenSource())
            {
                var ct = cTokenSource.Token;
                await SocketClient.OpenConnectionAsync(ct);
                while (true)
                {
                    ShowMenu();
                    string command = Console.ReadLine();

                    if (command == "4")
                    {
                        break;
                    }
                    if (command == "1")
                    {
                        await ShowAllUsers(ct);
                    }
                    if (command == "2")
                    {
                        await SendMessageToUser(ct);
                    }
                    if (command == "3")
                    {
                        await SendMessageToUser(ct);
                    }
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n\n ___________________________________\n" +
                                  " For show all users type 1," +
                                "\n For send message to a client type 2," +
                                "\n For show chats with client type 3" +
                                "\n For quite type 4" +
                                "\n ___________________________________");
        }
    }
}

