using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkServer
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private TcpListener server;
        private NetworkStream mainStream;
        BinaryFormatter binFormater;
        static Rectangle bounds;
        static Bitmap screen;
        private readonly Thread Listening;
        private string path;

        public Form1()
        {
            path = "C:\\Users\\Cyfralus\\Documents\\Visual Studio 2015\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\bin\\Debug\\picture.png";
            client = new TcpClient();
            Listening = new Thread(StartListening);
            binFormater = new BinaryFormatter();
            InitializeComponent();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            server = new TcpListener(IPAddress.Any, 8000);
            Listening.Start();
        }

        private void StartListening()
        {
            while (true)
            {
                while (!client.Connected)
                {
                    server.Start();
                    client = server.AcceptTcpClient();
                    MessageBox.Show("client");
                    BinaryFormatter binFormater = new BinaryFormatter();
                    if (client.Connected)
                    {
                        mainStream = client.GetStream();
                        screen = (Bitmap)binFormater.Deserialize(mainStream);
                        screen.Save(path, ImageFormat.Png);
                    }
                }
                client.Close();
            }
        }

        private void StopListening()
        {
            if (server != null)
            {
                server.Stop();
                client = null;
                if (Listening.IsAlive) Listening.Abort();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopListening();
        }

        private void buttonCatalog_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.SelectedPath + "\\picture.png";// придумать нумерование
                    textBox1.Text = path;
                }
            }
        }
    }
}
