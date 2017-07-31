using Evt.Communication.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryOutPoll
{
    class Program
    {
        static TcpClientEmm _client = new TcpClientEmm();
        static Tuple<string, int> _endPoint = new Tuple<string, int>("192.168.0.104", 31017);
        static void Main(string[] args)
        {
            _client.Connect(_endPoint.Item1, _endPoint.Item2);
            Console.WriteLine(_client.Client.Blocking);
            Console.WriteLine("Time  SelectRead   SelectWrite   SelectError   Connected");
            Observable.Interval(TimeSpan.FromSeconds(5)).Subscribe(s =>
            {
                bool r = _client.Client.Poll(10000, SelectMode.SelectRead);
                bool w = _client.Client.Poll(10000, SelectMode.SelectWrite);
                bool e = _client.Client.Poll(10000, SelectMode.SelectError);
                bool c = _client.Client.Connected;

                Console.WriteLine("{0}--   {1}    {2}     {3}    {4}",DateTime.Now.ToString("hh::mm:ss"),r,w,e,c);
            });

            Console.ReadLine();
        }
    }
}
