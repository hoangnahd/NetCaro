using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace caro_project
{
    internal class Server
    {
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<Player> players = new List<Player>();
        private bool isFull = false;
        
        public void Connect()
        {
            tcpListener = new TcpListener(IPAddress.Any, 1234);
            tcpListener.Start();

            // Start accepting client connections asynchronously
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void HandleClientConnection(IAsyncResult ar)
        {
            if (clients.Count >= 2)
            {
                TcpClient Client = tcpListener.EndAcceptTcpClient(ar);
                NetworkStream stream = Client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes("Room is full");
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                Client.Close();
                return; // If two clients are already connected, do not accept more connections
            }

            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            clients.Add(client);

            // Start listening for messages from the client
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            receiveThread.Start(client);

            // Continue accepting more client connections
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }


        private void ReceiveData(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[2048];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read data from the client
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    List<Player> jsonData = JsonConvert.DeserializeObject<List<Player>>(dataReceived);
                    if (players.Count != 2)
                    {
                        players.Add(jsonData[0]);
                    }
                    else
                    {
                        players = jsonData;
                        isFull = true;
                    }
                        

                    Broadcast(JsonConvert.SerializeObject(players));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }
            stream.Close();
            client.Close();
        }
        private void Broadcast(string message)
        {
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
