using Evt.Communication.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static bool _stop;
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(Test));
            t.Start();
            //ServerConnector connector = new ServerConnector();
            //connector.Start();
            Console.ReadLine();
            _stop = true;
            Console.ReadLine();

        }

        private static void Test()
        {
            while(!_stop)
            Console.WriteLine(DateTime.Now.ToString("mm:ss.fff"));
        }
    }
}
