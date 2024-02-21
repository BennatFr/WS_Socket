using System.Text;
using System.Net;
using System.Net.Sockets;
using Model;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Collections;

namespace Server {
    class Server {

        static void Main(string[] args) {
            Console.WriteLine("=== SERVER - 2 ===");
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
                var buffer = new byte[1_024 + 1_024 * 50];
                int received = client.Receive(buffer, SocketFlags.None);
                Console.WriteLine("Message reçu !");
                People deserializedPeople;
                using (MemoryStream ms = new MemoryStream(buffer, 0, received)) {
                    deserializedPeople = JsonSerializer.Deserialize<People>(ms);
                }

                Console.WriteLine("Message reçu: " + deserializedPeople.firstName + " " + deserializedPeople.lastName);

                deserializedPeople.id = 99;
                byte[] messageBytes;
                using (MemoryStream ms = new MemoryStream()) {
                    JsonSerializer.Serialize(ms, deserializedPeople);
                    messageBytes = ms.ToArray();
                }

                _ = client.Send(messageBytes, SocketFlags.None);
            }
        }

        private static void Deconnecter(Socket socket) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}