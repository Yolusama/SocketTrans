// See https://aka.ms/new-console-template for more information

using Common;
using KJSON;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

class Program
{
    class State
    {
        public bool Running { get; set; }
    }

    static void Main(string[] args)
    {
        Dictionary<int, Socket> sockets = new Dictionary<int, Socket>();
        object locker = new object();
        bool isRunning = true;
        const int BufferSize = 1024 * 1024 * 10;

        using Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        server.Listen(1000);
        Console.WriteLine($"Sokcet服务器运行：ip：{server.LocalEndPoint} or localhost:8080");

        ThreadPool.QueueUserWorkItem(arg =>
        {
            while (isRunning)
            {
                lock (locker)
                {
                    Socket client = server.Accept();
                    if (client == null || client.Available < 0) return;
                    IPEndPoint remotePort = client.RemoteEndPoint as IPEndPoint;
                    Console.WriteLine($"{remotePort}连接！");
                    sockets.Add(remotePort.Port, client);
                }
            }
        });

        ThreadPool.QueueUserWorkItem(arg =>
        {
            while (isRunning)
            {
                try
                {
                    for (int i = 0; i < sockets.Count; i++)
                    {
                        int port = sockets.Keys.ElementAt(i);
                        if(!sockets.ContainsKey(port))continue;
                        Socket client = sockets[port];
                        if (client.Connected && client.Available > 0)
                        {
                            byte[] buffer = new byte[BufferSize];
                            int recvBytes = client.Receive(buffer);
                            //if (recvBytes < 0) continue;
                            IPEndPoint remotePort = client.RemoteEndPoint as IPEndPoint;
                            string res = Encoding.UTF8.GetString(buffer, 0, recvBytes);
                            using JsonParser parser = new JsonParser(res);
                            MsgTransObj obj = parser.Parse<MsgTransObj>();
                            if (obj.Message != null)
                            {
                                if (obj.Port != 0 && sockets.ContainsKey(obj.Port))
                                {
                                    int portTo = obj.Port;
                                    obj.Port = remotePort.Port;
                                    sockets[portTo].Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
                                }
                            }
                            else
                            {
                                FileTransObj fileTransObj = parser.Parse<FileTransObj>();
                                if (fileTransObj.Port != 0 && sockets.ContainsKey(fileTransObj.Port))
                                    sockets[fileTransObj.Port]
                                        .Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(fileTransObj)));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        });

        ThreadPool.QueueUserWorkItem(arg =>
        {
            lock (locker)
            {
                for (int i = 0; i < sockets.Count; i++)
                {
                    int port = sockets.Keys.ElementAt(i);
                    Socket socket = sockets[port];
                    if (!socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
                        sockets.Remove(port);
                        socket.Close();
                        Console.WriteLine($"{iPEndPoint}断开连接！");
                    }
                }
            }
        });

        while (isRunning)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape||keyInfo.Key == ConsoleKey.Q)
            {
                isRunning = false;
                break;
            }
        }
    }
}