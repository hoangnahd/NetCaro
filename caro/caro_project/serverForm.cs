using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace caro_project
{
    public partial class serverForm : Form
    {
        public serverForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<Player> players = new List<Player>();

        public void Connect()
        {
            tcpListener = new TcpListener(IPAddress.Any, 1234);
            tcpListener.Start();

            // Start accepting client connections asynchronously
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void HandleClientConnection(IAsyncResult ar)
        {
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
                    players.Add(JsonConvert.DeserializeObject<List<Player>>(dataReceived)[0]);
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
