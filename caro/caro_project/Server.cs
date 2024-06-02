using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace caro_project
{
    internal class Server
    {
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<Player> players = new List<Player>();
        private bool isFull = false;
        private bool isRunning = false;

        public void Connect()
        {
            tcpListener = new TcpListener(IPAddress.Any, 1234);
            tcpListener.Start();
            isRunning = true;

            // Start accepting client connections asynchronously
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }

        private void HandleClientConnection(IAsyncResult ar)
        {
            if (clients.Count >= 2 || !isRunning)
            {
                TcpClient client = tcpListener.EndAcceptTcpClient(ar);
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes("Room is full");
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                client.Close();
                return; // If two clients are already connected, do not accept more connections
            }

            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
            clients.Add(tcpClient);

            // Start listening for messages from the client
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            receiveThread.Start(tcpClient);

            // Continue accepting more client connections
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }

        private void ReceiveData(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[2048];
            int bytesRead;

            while (isRunning)
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

        public void Stop()
        {
            // Stop the server
            isRunning = false;

            // Stop listening for new clients
            if (tcpListener != null)
            {
                tcpListener.Stop();
            }

            // Close all active client connections
            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                    {
                        client.GetStream().Close();
                        client.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error closing client: {ex.Message}");
                }
            }

            // Clear the client and player lists
            clients.Clear();
            players.Clear();
            isFull = false;
        }
    }
}
