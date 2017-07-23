using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evt.Communication.Connections
{
    public enum ServerName
    {
        OrderRoutingServer,
        VegaHitterServer,
        EvtVolPublisher,
        ComplianceServer,
        FuseServer
    }

    public struct ServerConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
    }
    
    
    
}
