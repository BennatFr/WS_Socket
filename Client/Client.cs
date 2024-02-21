using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Net;

namespace Client {
    class Client {
        static void Main(string[] args) {
            Console.WriteLine("=== CLIENT ===");
            Socket client = SeConnecter();
            
            EcouterReseau(client);
        }
        private static Socket SeConnecter() {
            Socket client = new(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090);

            
            client.Connect(ip);
            return client;
        }

        private static void EcouterReseau(Socket client) {
            while (true) {
                Console.Write("[SEND] ");
                // Send message.
                var message = Console.ReadLine();
                if (message == null) {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Message is null, skipping..."); // "Message is null, skipping...
                    Console.ResetColor();
                    continue;
                }
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = client.Send(messageBytes, SocketFlags.None);
                if (message == "exit") {
                    Deconnecter(client);
                    break;
                }

                // Receive ack.
                var buffer = new byte[1_024];
                var received = client.Receive(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"[RECEIVE] \"{response}\"");
                if (response == "exit") {
                    Deconnecter(client);
                    break;
                }
            }
        }

        private static void Deconnecter(Socket socket) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}