using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

namespace appConsoleSocketClient
{
    class Program
    {
        static Socket socket;
        const string IP = "192.168.56.1"; //"127.0.0.1"
        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs asyncArgs = new SocketAsyncEventArgs()
            {
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), 4502)
            };
            asyncArgs.Completed += OnConnected;
            socket.ConnectAsync(asyncArgs);
            Console.ReadLine();
            
        }

        private static void OnConnected(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OnConnected;
            if(e.SocketError== SocketError.Success)
            {
                SocketAsyncEventArgs asyncArgs = new SocketAsyncEventArgs();
                asyncArgs.Completed += (s, e1) => { socket.Close(); };
                byte[] bytes = Encoding.Unicode.GetBytes("This a test Socket data");
                asyncArgs.SetBuffer(bytes, 0, bytes.Length);
                socket.SendAsync(asyncArgs);
            }
            else
            {
                Console.WriteLine("Connection failed");
            }
        }
    }
}
