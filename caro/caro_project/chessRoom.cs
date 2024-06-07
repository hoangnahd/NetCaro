using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace caro_project
{
    public partial class chessRoom : Form
    {
        public chessRoom()
        { 
            InitializeComponent();
        }
        
        private TcpClient tcpClient;
        private List<Player> players = new List<Player>();
        private string auth;
        private NetworkStream stream;
        private Server server = new Server();
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void chessRoom_Load(object sender, EventArgs e)
        {
            connect2server();
            CreateChessBoard();
            InitializeCoolDownTimer();
        }
        public void Connect(string serverIp, int port)
        {
            try
            {
                tcpClient = new TcpClient("192.168.11.1", port);
               
                stream = tcpClient.GetStream();
                Console.WriteLine("Connected to server");

                auth = GenerateRandomString(20);
                players.Add(new Player(Form1.Username, "../../../icon/player2.jpg", false, auth));
                players[0].startCoolDown = true;

                string jsonData = JsonConvert.SerializeObject(players);
                SendData(jsonData);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.Start();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                MessageBox.Show("Failed to connect to the server. Starting a new server...", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Server server = new Server();
                server.Start();

                try
                {
                    tcpClient = new TcpClient("192.168.11.1", port);
                    stream = tcpClient.GetStream();
                    Console.WriteLine("Connected to new server");

                    auth = GenerateRandomString(20);
                    players.Add(new Player(Form1.Username, "../../../icon/player1.jpg", true, auth));
                    players[0].startCoolDown = true;

                    string jsonData = JsonConvert.SerializeObject(players);
                    SendData(jsonData);

                    Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                    receiveThread.Start();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Failed to connect to new server: {ex2.Message}");
                    MessageBox.Show("Failed to start a new server and connect to it.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                MessageBox.Show("An unexpected error occurred while trying to connect to the server.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void connect2server()
        {
            Username.Enabled = false;
            Username.Text = Form1.Username;

            string serverIp = GetLocalIPAddress();
            MessageBox.Show(serverIp);
            txbIp.Text = serverIp;
            Connect(serverIp, 1234);
        }

        private string GetLocalIPAddress()
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

        private void InitializeCoolDownTimer()
        {
            prbCoolDown.Step = cons.coolDownstep;
            prbCoolDown.Maximum = cons.coolDownTime;
            tmCoolDown.Interval = cons.coolDownInterval;
            prbCoolDown.Value = 0;
            tmCoolDown.Start();
        }
        private void SendData(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        private void ReceiveData()
        {
            byte[] buffer = new byte[2048];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read data from the server
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (dataReceived == "Room is full")
                    {
                        MessageBox.Show("Room is full");
                        Application.Exit();
                    }
                    players = JsonConvert.DeserializeObject<List<Player>>(dataReceived);
                    if(players.Count == 2)
                    {
                        if (players[0].startCoolDown && players[1].startCoolDown)
                        {
                            prbCoolDown.Value = 0;
                            InitializeCoolDownTimer();
                            players[0].startCoolDown = false;
                            players[1].startCoolDown = false;
                        }
                        MarkButton(players[0], players[1]);
                        if (players[0].isWin)
                        {
                            MessageBox.Show(players[0].Name + " win!!");
                            Application.Exit();
                        }
                        else if (players[1].isWin)
                        {
                            MessageBox.Show(players[1].Name + " win!!");
                            Application.Exit();
                        }
                        if (players[0].isActive)
                            displayCurrPlayer(players[0]);
                        else
                            displayCurrPlayer(players[1]); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }

            stream.Close();
            tcpClient.Close();
        }   
        private void CreateChessBoard()
        {
            for (int row = 0; row < cons.chessBoardHeight; row++)
            {
                for (int col = 0; col < cons.chessBoardWidth; col++)
                {
                    Button button = new Button();
                    button.Size = new Size(cons.chessSize, cons.chessSize);
                    button.Location = new Point(col * cons.chessSize, row * cons.chessSize);
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    // Set the color
                    if ((row + col) % 2 == 0)
                    {
                        button.BackColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.WhiteSmoke;
                    }

                    // Attach MouseEnter and MouseLeave events
                    button.MouseEnter += (sender, e) => Button_MouseEnter(sender, e, button);
                    button.MouseLeave += (sender, e) => Button_MouseLeave(sender, e, button);
                    button.Click += (sender, e) => Button_Click(sender, e, button);
                    button.TabStop = false;
                    // Add the button to the panel
                    this.chessBoard.Controls.Add(button);
                }
            }
        }
        private void Button_MouseEnter(object sender, EventArgs e, Button button)
        {
            // Change the background color to blue with a "blur" effect
            button.BackColor = Color.FromArgb(100, 0, 0, 255); // Semi-transparent blue
        }
        private void Button_MouseLeave(object sender, EventArgs e, Button button)
        {
            // Change the background color back to its original color
            int row = button.Location.Y / button.Height;
            int col = button.Location.X / button.Width;
            // Set the color
            if ((row + col) % 2 == 0)
            {
                button.BackColor = Color.White;
            }
            else
            {
                button.BackColor = Color.WhiteSmoke;
            }
        }
        private void displayCurrPlayer(Player currPlayer)
        {
            txbPlayerName.Text = currPlayer.Name;
            pcbAvt.Image = Image.FromFile(currPlayer.Mark);
        }
        private Button GetButtonAtPosition(int row, int col)
        {
            foreach (Control control in chessBoard.Controls)
            {
                if (control is Button button)
                {
                    int buttonRow = button.Top / button.Height;
                    int buttonCol = button.Left / button.Width;

                    if (buttonRow == row && buttonCol == col)
                    {
                        return button;
                    }
                }
            }

            return null; // Button not found at the specified position
        }
        private int CountConsecutive(List<int> list, int value)
        {
            int count = 1;
            int nextValue = value + 1;
            while (list.Contains(nextValue))
            {
                nextValue++;
                count++;
            }

            int prevValue = value - 1;
            while (list.Contains(prevValue))
            {
                prevValue--;
                count++;
            }

            return count;
        }
        private void MarkButton(Player p1, Player p2)
        {
            foreach(var row in p1.row2col)
            {
                foreach(var col in row.Value)
                {
                    Button btn = GetButtonAtPosition(row.Key, col);
                    btn.Enabled = false;
                    btn.BackgroundImage = Image.FromFile("../../../icon/iconX.jpg");
                }    
            }
            foreach (var row in p2.row2col)
            {
                foreach (var col in row.Value)
                {
                    Button btn = GetButtonAtPosition(row.Key, col);
                    btn.Enabled = false;
                    btn.BackgroundImage = Image.FromFile("../../../icon/iconY.jpg");
                }
            }
        }
        private int CheckDiagonal(Player player, int row, int col)
        {
            int count = 1;

            // Check top-left to bottom-right diagonal
            int nextRow = row - 1;
            int nextCol = col - 1;
            while (player.row2col.ContainsKey(nextRow) && player.row2col[nextRow].Contains(nextCol))
            {
                nextRow--;
                nextCol--;
                count++;
            }

            nextRow = row + 1;
            nextCol = col + 1;
            while (player.row2col.ContainsKey(nextRow) && player.row2col[nextRow].Contains(nextCol))
            {
                nextRow++;
                nextCol++;
                count++;
            }

            if (count >= 5)
            {
                return count;
            }

            // Check top-right to bottom-left diagonal
            count = 1;
            nextRow = row + 1;
            nextCol = col - 1;
            while (player.row2col.ContainsKey(nextRow) && player.row2col[nextRow].Contains(nextCol))
            {
                nextRow++;
                nextCol--;
                count++;
            }

            nextRow = row - 1;
            nextCol = col + 1;
            while (player.row2col.ContainsKey(nextRow) && player.row2col[nextRow].Contains(nextCol))
            {
                nextRow--;
                nextCol++;
                count++;
            }

            return count;
        }
        private bool IsWin(Player player, int row, int col)
        {
            return CountConsecutive(player.row2col[row], col) >= 5 ||
                   CountConsecutive(player.col2row[col], row) >= 5 ||
                   CheckDiagonal(player, row, col) >= 5;
        }
        private void Button_Click(object sender, EventArgs e, Button button)
        {
            if (players.Count != 2)
                return;
            int row = button.Top / button.Height;
            int col = button.Left / button.Width;
            if (players[0].isActive && players[0].auth == auth)
            {
                players[0].isActive = false;
                players[1].isActive = true;
                // Log the move for player 1
                if (!players[0].row2col.ContainsKey(row))
                    players[0].row2col[row] = new List<int>();
                players[0].row2col[row].Add(col);

                if (!players[0].col2row.ContainsKey(col))
                    players[0].col2row[col] = new List<int>();
                players[0].col2row[col].Add(row);
                if (IsWin(players[0], row, col))
                    players[0].isWin = true;
                players[0].startCoolDown = true;
                players[1].startCoolDown = true;
                SendData(JsonConvert.SerializeObject(players));
            }
            else if(players[1].auth == auth && players[1].isActive)
            {
                players[1].isActive = false;
                players[0].isActive = true;
                // Log the move for player 2
                if (!players[1].row2col.ContainsKey(row))
                    players[1].row2col[row] = new List<int>();
                players[1].row2col[row].Add(col);

                if (!players[1].col2row.ContainsKey(col))
                    players[1].col2row[col] = new List<int>();
                players[1].col2row[col].Add(row);
                if (IsWin(players[1], row, col))
                    players[1].isWin = true;
                players[0].startCoolDown = true;
                players[1].startCoolDown = true;
                SendData(JsonConvert.SerializeObject(players));
            }
            
        }
        private void chessRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            var openForms = Application.OpenForms.Cast<Form>().ToList();

            // Close each form
            foreach (var form in openForms)
            {
                form.Close();
            }

            // Exit the application
            Application.Exit();
        }
        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            if (prbCoolDown.Value == prbCoolDown.Maximum && players.Count == 2)
            {
                players[0].startCoolDown = true;
                players[1].startCoolDown = true;
                if (players[0].isActive)
                {
                    players[0].isActive = false;
                    players[1].isActive = true;
                    SendData(JsonConvert.SerializeObject(players));
                }
                else
                {
                    players[1].isActive = false;
                    players[0].isActive = true;
                    // Switch to player 1
                    SendData(JsonConvert.SerializeObject(players));
                }
                prbCoolDown.Value = 0;
            }
            else
            {
                // Increment the progress bar
                prbCoolDown.PerformStep();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
