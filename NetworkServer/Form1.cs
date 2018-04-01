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
        ImageFormat format;
        string fileFormat, name;
        static Bitmap screen;
        private readonly Thread Listening;
        private string path;

        public Form1()
        {
            path = Application.StartupPath + "\\";
            client = new TcpClient();
            Listening = new Thread(StartListening);
            binFormater = new BinaryFormatter();
            InitializeComponent();
            textBox1.Text = path;
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new TcpListener(IPAddress.Any, 8000);
                MessageBox.Show("Сервер запущен");
            }
            if (!Listening.IsAlive)
                Listening.Start();
            else
                MessageBox.Show("Сервер уже запущен");
        }

        private void StartListening()
        {
            while (true)
            {
                while (!client.Connected)
                {
                    server.Start();
                    client = server.AcceptTcpClient();
                    BinaryFormatter binFormater = new BinaryFormatter();
                    if (client.Connected)
                    {
                        mainStream = client.GetStream();
                        screen = (Bitmap)binFormater.Deserialize(mainStream);
                        name = (string)binFormater.Deserialize(mainStream);
                        fileFormat = (string)binFormater.Deserialize(mainStream);
                        GetFormat(fileFormat);
                        screen.Save(path + name + fileFormat, format);
                        binFormater.Serialize(mainStream, path + name);
                    }
                }
                client.Close();
            }
        }

        private void GetFormat(string strformat)
        {
            switch (strformat)
            {
                case ".jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    format = ImageFormat.Png;
                    break;
                case ".png":
                    format = ImageFormat.Png;
                    break;
                case ".gif":
                    format = ImageFormat.Gif;
                    break;
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
                    path = dialog.SelectedPath + "\\";
                    textBox1.Text = path;
                }
            }
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            StopListening();
            MessageBox.Show("Сервер остановлен");
        }
    }
}
