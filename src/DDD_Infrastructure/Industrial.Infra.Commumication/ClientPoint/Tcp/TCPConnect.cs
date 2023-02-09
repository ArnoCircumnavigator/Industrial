using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Industrial.Infra.Commumication
{
    /// <summary>
    /// TCP连接 
    /// 从c/s的角度，这个对象更偏向于客户端
    /// </summary>
    internal class TCPConnect : IConnect
    {
        readonly string _host = "localhost";
        readonly int _port = 0;
        ConnectStatus status;
        public TCPConnect(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ConnectStatus GetStatus()
        {
            return status;
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            var hostEntry = new DnsEndPoint(_host, _port);
            args.RemoteEndPoint = hostEntry;
            if (socket.ConnectAsync(args))
                status = ConnectStatus.Connecting;
            else
            {
                status = ConnectStatus.Closed;
                return;
            }
        }

        public bool TryGetSession(out ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
