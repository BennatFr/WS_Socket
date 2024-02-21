using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using Model;
using System.Collections;
using System.Text.Json;

namespace Client {
    class Client {
        static void Main(string[] args) {
            Console.WriteLine("=== CLIENT 2===");
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
                People people = new People();
                Console.Write("[firstName] ");
                var message = Console.ReadLine();
                people.firstName = message;

                Console.Write("[lastName] ");
                message = Console.ReadLine();
                people.lastName = message;

                byte[] byteArray;
                using (MemoryStream ms = new MemoryStream()) {
                    JsonSerializer.Serialize(ms, people);
                    byteArray = ms.ToArray();
                }
                _ = client.Send(byteArray, SocketFlags.None);

                // Receive ack.
                var buffer = new byte[1_024];
                int received = client.Receive(buffer, SocketFlags.None);
                People deserializedPeople;
                using (MemoryStream ms = new MemoryStream(buffer, 0, received)) {
                    deserializedPeople = JsonSerializer.Deserialize<People>(ms);
                }

                Console.WriteLine($"[RECEIVE] \"{deserializedPeople.firstName} {deserializedPeople.lastName} {deserializedPeople.id}\"");
            }
        }

        private static void Deconnecter(Socket socket) {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}