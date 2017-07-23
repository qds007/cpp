using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Evt.Communication.Connections
{
    public class TcpClientFactory
    {
        Dictionary<ServerName, TcpClient> _serverClients = new Dictionary<ServerName, TcpClient>();

        public void Start()
        {
            foreach (var n in Enum.GetValues(typeof(ServerName)))
            {
                ServerName serverName;
                Enum.TryParse<ServerName>(n.ToString(), out serverName);
                var client = BuildServerClientByName(serverName);
                if (client != null)
                    _serverClients.Add(serverName, client);
            }
        }

        private TcpClient BuildServerClientByName(ServerName serverName)
        {
            var t = new TcpClient(AddressFamily.InterNetwork);
            t.Connect("hostname", 12345);
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            return null;
        }

        Dictionary<ServerName, ServerConfig> _serverConfigs = new Dictionary<ServerName, ServerConfig>()
        {
            { ServerName.OrderRoutingServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31017 } },
            { ServerName.VegaHitterServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31224 } },
            { ServerName.EvtVolPublisher, new ServerConfig() {HostName="eddcr-emm01",Port=25249 } },
            { ServerName.ComplianceServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31019 } },
            { ServerName.FuseServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31013} }
        };
    }
}
