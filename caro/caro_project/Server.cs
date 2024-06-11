using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace caro_project
{
    internal class Server
    {
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<Player> players = new List<Player>();
        private bool isFull = false;
        public void Start(string serverIp)
        {
            tcpListener = new TcpListener(IPAddress.Parse(serverIp), cons.port);
            tcpListener.Start();
            Console.WriteLine("Server started, waiting for clients...");

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
            else
            {
                TcpClient client = null;
                client = tcpListener.EndAcceptTcpClient(ar);
                // Add client to list and start a new thread to receive data
                clients.Add(client);
                Console.WriteLine("Client connected.");

                Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
                receiveThread.Start(client);
            }

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
                    if (bytesRead == 0)
                    {
                        // Connection closed
                        break;
                    }

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

            // Clean up when the client disconnects
            stream.Close();
            client.Close();
            clients.Remove(client);
            Console.WriteLine("Client disconnected");
        }
        private void Broadcast(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (TcpClient client in clients)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Broadcast error: {ex.Message}");
                }
            }
        }
        public string GetLocalIPAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");

        }
    }
}
