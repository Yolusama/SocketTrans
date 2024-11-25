using Common;
using KJSON;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SocketClient
{
    public partial class Form1 : Form
    {
        private Socket socket;
        private Thread recvThread;
        const int BufferSize = 1024*1024*10;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
            serverBase.Text = socket.LocalEndPoint.ToString();

            recvThread = new Thread(() =>
            {
                while (socket.Connected)
                {
                    string recvStr = "";
                    try
                    {
                        if (socket.Available <= 0) continue;
                        byte[] buffer = new byte[BufferSize];
                        int recvBytes = socket.Receive(buffer);
                        recvStr = Encoding.UTF8.GetString(buffer, 0, recvBytes);
                        using JsonParser parser = new JsonParser(recvStr);
                        MsgTransObj obj = parser.Parse<MsgTransObj>();
                        if (obj.Message != null)
                        {
                            content.AppendText($"{obj.Port}：{obj.Message}\n");
                        }
                        else
                        {
                            FileTransObj fileTrans = parser.Parse<FileTransObj>();
                            File.WriteAllBytes(fileTrans.FileName, fileTrans.Data);
                            IPEndPoint point = socket.LocalEndPoint as IPEndPoint;
                            content.AppendText($"{point.Port}：文件已接收！\n");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                }
            });

            recvThread.Start();

        }
        private void send_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                MsgTransObj obj = new MsgTransObj();
                int port = 0;
                int.TryParse(portTo.Text, out port);
                obj.Port = port;
                obj.Message = toSend.Text;
                socket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
                content.AppendText($"{endPoint.Port}：{toSend.Text}\n");
                toSend.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void sendFile_Click(object sender, EventArgs e)
        {
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                FileInfo file = new FileInfo(fileName);
                using FileStream stream = file.OpenRead();
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[BufferSize];
                int port = 0;
                int.TryParse(portTo.Text,out port);
                int bytesRead;
                while ((bytesRead = stream.Read(buffer)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                        data.Add(buffer[i]);
                }

                FileTransObj obj = new FileTransObj
                {
                    FileName = dialog.SafeFileName,
                    Port = port,
                    Data = data.ToArray()
                };
                socket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
                content.AppendText($"{endPoint.Port}：{file.Name}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            recvThread.Join();
        }
    }
}