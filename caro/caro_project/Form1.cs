using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace caro_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static public string Username;
        static public int roomId;
        private Dictionary<int, int> room2port = new Dictionary<int, int>();
        
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            room2port[1] = 1234;
            room2port[2] = 80;

            if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please enter your username or select a room!");
                return;
            }

            int selectRoom = int.Parse(comboBox1?.SelectedItem?.ToString());
            roomId = room2port[selectRoom];
            Username = textBox1.Text;
            chessRoom room = new chessRoom();
            room.ShowDialog();
        }
    }
}
