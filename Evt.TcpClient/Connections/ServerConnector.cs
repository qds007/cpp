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
        const int CONN_CHK_INTERVAL_SEC = 10;
        const int START_RETRY_INTERVAL_SEC = 10;
        Dictionary<ServerName, TcpClient> _serverClients = new Dictionary<ServerName, TcpClient>();

        public void Start()
        {
            foreach (var n in Enum.GetValues(typeof(ServerName)))
            {
                ServerName serverName;
                Enum.TryParse<ServerName>(n.ToString(), out serverName);
                TryBuildServerClientByName(serverName);
            }
            SetupCheckAndRestoreConnections();
        }

        private void TryBuildServerClientByName(ServerName serverName)
        {
            var client = BuildServerClientByName(serverName);
            if (client != null)
            {
                _serverClients.Add(serverName, client);
                return;
            }

            Observable.Interval(TimeSpan.FromSeconds(START_RETRY_INTERVAL_SEC)).TakeWhile(s=>client==null).Subscribe(s =>
            {
                client = BuildServerClientByName(serverName);
                if (client != null)
                {
                    _serverClients.Add(serverName, client);
                }
            });
        }

        void SetupCheckAndRestoreConnections()
        {
            Observable.Interval(TimeSpan.FromSeconds(CONN_CHK_INTERVAL_SEC)).Subscribe(s =>
            {
                for (int i=0; i<_serverClients.Count;i++)
                {
                    try
                    {
                        var sc = _serverClients.ElementAt(i);
                        bool isConnected = sc.Value!=null && IsConnected(sc.Value.Client, sc.Key);

                        LOG(string.Format("{0} {1}", sc.Key, isConnected));
                        if (!isConnected)
                        {
                            LOG(string.Format("try reconnecting {0}...", sc.Key));
                            if (sc.Value != null)
                              sc.Value.Client.Shutdown(SocketShutdown.Both); 

                            _serverClients[sc.Key] = BuildServerClientByName(sc.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG(string.Format("{0} {1}", ex.Message, ex.StackTrace));
                    }
                }
            });
        }

        private bool IsConnected(Socket sock, ServerName serverName)
        {
            try
            {
                if (!sock.Connected)
                {
                    LOG(string.Format("{0} Desktop Tcp caller has a problem and fail to connect", serverName));
                    return false;
                }

                if (!IsPhysicalLevelConnected(sock))
                {
                    LOG(string.Format("{0} ping failed, could be Physical Server rebooting.", serverName));
                    return false;
                }

                if (!IsApplicationLevelConnected(sock))
                {
                    var ep = sock.RemoteEndPoint as IPEndPoint;
                    LOG(string.Format("{0} not responding from {1}:{2}, it could be restarting.", serverName, ep.Address, ep.Port));
                    return false;
                }
            }
            catch(Exception ex)
            {
                LOG(string.Format("{0} {1}", ex.Message, ex.StackTrace));
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

        private TcpClient BuildServerClientByName(ServerName serverName)
        {
            try
            {
                var t = new TcpClient();
                var hostNameOrIP = _serverConfigs[serverName].HostName;
                var port = _serverConfigs[serverName].Port;
                t.Connect(hostNameOrIP, port);
                return t;
            }
            catch(Exception ex)
            {
                LOG(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            }

            return null;
        }

        Dictionary<ServerName, ServerConfig> _serverConfigs = new Dictionary<ServerName, ServerConfig>()
        {
            //{ ServerName.OrderRoutingServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31017 } },
            //{ ServerName.VegaHitterServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31224 } },
            //{ ServerName.EvtVolPublisher, new ServerConfig() {HostName="eddcr-emm01",Port=25249 } },
            //{ ServerName.ComplianceServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31019 } },
            //{ ServerName.FuseServer, new ServerConfig() {HostName="eqpwk-emm13",Port=31013} }

            { ServerName.OrderRoutingServer, new ServerConfig() {HostName="192.168.0.104",Port=31017 } },
            { ServerName.VegaHitterServer, new ServerConfig() {HostName="192.168.0.104",Port=31224 } },
            { ServerName.EvtVolPublisher, new ServerConfig() {HostName="192.168.0.104",Port=25249 } },
            { ServerName.ComplianceServer, new ServerConfig() {HostName="192.168.0.104",Port=31019 } },
            { ServerName.FuseServer, new ServerConfig() {HostName="192.168.0.104",Port=31013} }
        };

        static void LOG(string msg)
        {
            Debug.WriteLine(msg);
            Console.WriteLine(msg);
        }
    }
}


//const int POLL_DURATION_MS = 100;
//bool IsReadable(Socket sock)
//{
//    if (!sock.Connected) return false;
//    bool readable = sock.Poll(POLL_DURATION_MS, SelectMode.SelectRead);
//    bool hasDataToRead = sock.Available > 0;
//    //  Debug.WriteLine("readable {0} hasDataToRead {1}", readable, hasDataToRead);
//    return readable && hasDataToRead;
//} 

//bool isReadable = IsReadable(sc.Value.Client);
//bool isWritable = sc.Value.Client.Poll(POLL_DURATION_MS, SelectMode.SelectWrite);
//bool hasError = sc.Value.Client.Poll(POLL_DURATION_MS, SelectMode.SelectWrite);
//            if (!isConnected || !isReadable || !isWritable || hasError)
//            {
//            Debug.WriteLine("{0} {1} {2} {3} {4} ", sc.Key,
//                isConnected ? "Connected" : "Disconnected",
//                isReadable ? "readable" : "notReadable", "", "");
//            isWritable ? "writable" : "notWritable",
//                hasError ? "noError" : "hasError");
//}
