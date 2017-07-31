using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evt.Communication.Connections
{
    /// <summary>
    /// TcpClient Extended with Task based send and read, and server heartbeat
    /// </summary>
    public class TcpClientEmm : TcpClient
    {
        const int READ_CHUNK_SIZE = 1024;
        const int SERVER_HEARTBEAT_SEC = 60;
        Socket _sock;
        NetworkStream _stream;
        volatile bool _shutdownRequested;

        public new void Connect(string hostName, int port)
        {
            _sock = Client;
            _stream = GetStream();
            SetupServerDataStream();
            StartReadingServerHeartbeat();
            _sock.Connect(hostName, port);
        }

        private void StartReadingServerHeartbeat()
        {
            Observable.TimeInterval<DateTime>(ServerDataStream.Select(s => DateTime.Now))
                .SubscribeOn(new EventLoopScheduler())
                .Subscribe(ts => {
                    if (ts.Interval.Seconds> SERVER_HEARTBEAT_SEC )
                        {
                            _shutdownRequested = true;
                        }
                 });
        }

        IObservable<byte[]> ServerDataStream { get;  set; }
        IObserver<byte[]> _serverDataObserver;
        void SetupServerDataStream()
        {
            ServerDataStream = Observable.Create<byte[]>(observer =>
            {
                _serverDataObserver = observer;
                
                while(!_shutdownRequested)
                {
                    byte[] data = new byte[READ_CHUNK_SIZE];
                    while(_stream.DataAvailable)
                    {
                        _stream.ReadAsync(data, 0, READ_CHUNK_SIZE);
                        _serverDataObserver.OnNext(data);
                    }
                }
                
                return Disposable.Create(()=>
                {
                    _serverDataObserver = null;
                });
            });
        }

        public void RequestShutdown()
        {
            _shutdownRequested = true;
            _sock.Shutdown(SocketShutdown.Both);
        }

        public Task  WriteAsync(int id,IMessage proto )
        {
            int size;
            byte[] bytes = ProtoToBytes(proto, out size);

            //write id->length->proto
            return _stream.WriteAsync(IntTo4Bytes(id), 0, 4)
                  .ContinueWith(_ => _stream.WriteAsync(IntTo4Bytes(size), 0, 4))
                  .ContinueWith(_ => _stream.WriteAsync(bytes, 0, size));           
        }

        byte[] IntTo4Bytes(int i)
        {
            return BitConverter.GetBytes(i);
        }
        
        byte[] ProtoToBytes(IMessage msg,out int size)
        {
            size = msg.CalculateSize();
            byte[] bytes = new byte[size];
            CodedOutputStream coStream = new CodedOutputStream(bytes);
            msg.WriteTo(coStream);
            return bytes;
        }
    }
}
