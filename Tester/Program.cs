using Evt.Communication.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConnector connector = new ServerConnector();
            connector.Start();
            Console.ReadLine();
        }
    }
}
