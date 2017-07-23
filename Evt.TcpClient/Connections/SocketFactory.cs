using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace Evt.Communication.Connections
{
    public class SocketFactory
    {
        Dictionary<ServerName, Socket> _sockets= new Dictionary<ServerName, Socket>();
        public void Start()
        {
            foreach (var n in Enum.GetValues(typeof(ServerName)))
            {
                ServerName serverName;
                Enum.TryParse<ServerName>(n.ToString(), out serverName);
                var sock = BuildSocketByName(serverName);
                if(sock!=null)
                _sockets.Add(serverName, sock);
            }
        }
        Socket BuildSocketByName(ServerName name)
        {
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var ipEP = BuildIPEndPointFromConfig(name);

            //connect and set up retry/poll
            sock.Connect(ipEP);
            if(!sock.Poll(60000,SelectMode.SelectError))
            {
                sock.Connect(ipEP);
            }

            return null;
        }

        IPEndPoint BuildIPEndPointFromConfig(ServerName name)
        {
            string hostName = "eqpwk-emm13";
            switch (name)
            {
                case ServerName.OrderRoutingServer:
                    return new IPEndPoint(DnsResolveToIP(hostName), 31017);
                case ServerName.VegaHitterServer:
                    return new IPEndPoint(DnsResolveToIP(hostName), 31224);
                case ServerName.EvtVolPublisher:
                    hostName = "eddcr-emm01";
                    return new IPEndPoint(DnsResolveToIP(hostName), 25249);
                case ServerName.ComplianceServer:
                    return new IPEndPoint(DnsResolveToIP(hostName), 31019);
                case ServerName.FuseServer:
                    return new IPEndPoint(DnsResolveToIP(hostName), 31013);
            }
            return null;
        }
        IPAddress DnsResolveToIP(string hostName)
        {
            IPAddress[] iPs = Dns.GetHostAddresses(hostName);
            foreach(var ip in iPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return null;
        }
    }
}
