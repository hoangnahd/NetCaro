using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace caro_project
{
    internal class Server
    {
        private TcpListener tcpListener;
        private ConcurrentDictionary<TcpClient, NetworkStream> clients = new ConcurrentDictionary<TcpClient, NetworkStream>();
        private List<Player> players = new List<Player>();
        private bool isFull = false;

        public void Connect()
        {
            tcpListener = new TcpListener(IPAddress.Any, 1234); // Bind to all available network interfaces on port 1234
            tcpListener.Start(); // Start listening for incoming connections

            Console.WriteLine("Server started. Waiting for clients...");
            AcceptClients(); // Start accepting client connections
        }

        private void AcceptClients()
        {
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }

        private void HandleClientConnection(IAsyncResult ar)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            NetworkStream stream = client.GetStream();

            if (clients.Count >= 2)
            {
                byte[] buffer = Encoding.UTF8.GetBytes("Room is full");
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                client.Close();
            }
            else
            {
                if (clients.TryAdd(client, stream))
                {
                    Console.WriteLine("Client connected.");
                    // Start listening for messages from the client
                    Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
                    receiveThread.Start(client);
                }
            }

            AcceptClients(); // Continue accepting more client connections
        }

        private void ReceiveData(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = clients[client];

            byte[] buffer = new byte[2048];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    List<Player> jsonData = JsonConvert.DeserializeObject<List<Player>>(dataReceived);
                    lock (players)
                    {
                        if (players.Count < 2)
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Clean up when client disconnects
                Console.WriteLine("Client disconnected.");
                stream.Close();
                client.Close();
                clients.TryRemove(client, out _);
            }
        }

        private void Broadcast(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients.Keys)
            {
                NetworkStream stream = clients[client];
                try
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Broadcast error: {ex.Message}");
                }
            }
        }
    }
}