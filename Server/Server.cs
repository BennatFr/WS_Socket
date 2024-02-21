using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server {
    class Server {

        static void Main(string[] args) {
            Console.WriteLine("=== SERVER ===");
            Socket socket = SeConnecter();

            var handler = AccepterConnexion(socket);

            EcouterReseau(handler);        
        }

        private static Socket SeConnecter() {
            Socket socket = new(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090);
            socket.Bind(ip);
            socket.Listen(100);
            return socket;
        }

        private static Socket AccepterConnexion(Socket socket) {
            var handler = socket.Accept();
            Console.WriteLine("Connexion établie avec " + handler.RemoteEndPoint);
            return handler;
        }


        private static void EcouterReseau(Socket client) {
            while (true) {
                var buffer = new byte[1_024];
                var received = client.Receive(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine("[RECEIVE] " + response);
                if (response == "exit") {
                    Deconnecter(client);
                    break;
                }

                Console.Write("[SEND] ");
                var message = Console.ReadLine();
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = client.Send(messageBytes, SocketFlags.None);
                if (message == "exit") {
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