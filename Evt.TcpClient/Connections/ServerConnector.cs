using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evt.Communication.Connections
{
    public class ServerConnector
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
            SetupCheckAndRestoreConnections();
        }


        const int POLL_INTERVAL_SEC = 10;
        const int POLL_DURATION_MS = 100;
        void SetupCheckAndRestoreConnections()
        {
            Observable.Interval(TimeSpan.FromSeconds(POLL_INTERVAL_SEC)).Subscribe(s =>
            {
                foreach (var sc in _serverClients)
                {
                    bool isConnected = IsConnected(sc.Value.Client, sc.Key);
                    bool isReadable = IsReadable(sc.Value.Client);
                    bool isWritable = sc.Value.Client.Poll(POLL_DURATION_MS, SelectMode.SelectWrite);
                    bool hasError = sc.Value.Client.Poll(POLL_DURATION_MS, SelectMode.SelectWrite);
                    Debug.WriteLine("{0} {1}", sc.Key, isConnected);
                    if (!isConnected || !isReadable || !isWritable || hasError)
                    {
                        //Debug.WriteLine("{0} {1} {2} {3} {4} ", sc.Key,
                        //    isConnected ? "Connected" : "Disconnected",
                        //    isReadable?"readable":"notReadable", "", "");
                        ////isWritable?"writable":"notWritable",
                        ////hasError?"noError":"hasError");
                    }
                }
            });



        }

        private bool IsConnected(Socket sock, ServerName serverName)
        {
            if(!sock.Connected)
            {
                Debug.WriteLine("{0} Desktop Tcp caller has a problem and fail to connect", serverName);
                return false;
            }

            if (!IsPhysicalLevelConnected(sock))
            {
                Debug.WriteLine("{0} ping failed, could be Physical Server rebooting.", serverName);
                return false;
            }

            if(!IsApplicationLevelConnected(sock))
            {
                var ep = sock.RemoteEndPoint as IPEndPoint;
                Debug.WriteLine("{0} not responding from {1}:{2}, it could be restarting.", serverName,ep.Address,ep.Port  );
                return false;
            }

            return true;
        }

        private bool IsPhysicalLevelConnected(Socket sock)
        {
            if (!sock.Connected) return false;
            Ping p = new Ping();
            var ep = (IPEndPoint)sock.RemoteEndPoint;
            PingReply reply = p.Send(ep.Address);
            return reply.Status == IPStatus.Success;
        }

        public bool IsApplicationLevelConnected(Socket sock)
        {
            bool blockingStateSaved = sock.Blocking;

            try
            {
                sock.Blocking = false;
                sock.Send(new byte[1], 1,SocketFlags.None);
                return true;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035)) // 10035 == WSAEWOULDBLOCK
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                sock.Blocking = blockingStateSaved;
            }
        }

        bool IsReadable(Socket sock)
        {
            if (!sock.Connected) return false;
            bool readable =sock.Poll(POLL_DURATION_MS, SelectMode.SelectRead);
            bool hasDataToRead = sock.Available>0;
          //  Debug.WriteLine("readable {0} hasDataToRead {1}", readable, hasDataToRead);
            return readable && hasDataToRead;
        }

        private TcpClient BuildServerClientByName(ServerName serverName)
        {
            var t = new TcpClient();
            var hostNameOrIP = _serverConfigs[serverName].HostName;
            var port = _serverConfigs[serverName].Port;
            t.Connect(hostNameOrIP,port);
 
            return t;
        }

        Dictionary<ServerName, ServerConfig> _serverConfigs = new Dictionary<ServerName, ServerConfig>()
        {
            //{ ServerName.OrderRoutingServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31017 } },
            //{ ServerName.VegaHitterServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31224 } },
            //{ ServerName.EvtVolPublisher, new ServerConfig() {HostName="eddcr-emm01",Port=25249 } },
            //{ ServerName.ComplianceServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31019 } },
            //{ ServerName.FuseServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31013} }

            { ServerName.OrderRoutingServer, new ServerConfig() {HostName="192.168.0.101",Port=31017 } },
            { ServerName.VegaHitterServer, new ServerConfig() {HostName="192.168.0.101",Port=31224 } },
            { ServerName.EvtVolPublisher, new ServerConfig() {HostName="192.168.0.101",Port=25249 } },
            { ServerName.ComplianceServer, new ServerConfig() {HostName="192.168.0.101",Port=31019 } },
            { ServerName.FuseServer, new ServerConfig() {HostName="192.168.0.101",Port=31013} }
        };
    }
}
