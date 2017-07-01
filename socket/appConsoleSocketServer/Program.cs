using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace appConsoleSocketServer
{
    class Program
    {
        static Socket socket;
        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 4502));

            socket.Listen(100); 

            while(true)
            {
               Socket s= socket.Accept();
                Thread t = new Thread(() =>
                 {
                     byte[] buf = new byte[1024];
                     int recvLen=s.Receive(buf);
                     Console.WriteLine(Encoding.Unicode.GetChars(buf,0, recvLen));
                     s.Close();
                 });
                t.Start();
            }
        }
    }
}
