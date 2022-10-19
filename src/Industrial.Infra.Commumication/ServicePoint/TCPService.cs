using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Industrial.Infra.Commumication
{
    public class TCPService : ICommumicationServicePoint
    {
        readonly string _host = "localhost";
        readonly int _port = 0;
        readonly int _maxClientCotunt = 0;
        ServiceStatus status;
        Socket socket;
        public TCPService(string host, int port, int maxClientCount)
        {
            _host = host;
            _port = port;
            _maxClientCotunt = maxClientCount;
        }

        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var hostEntry = new DnsEndPoint(_host, _port);
            socket.Bind(hostEntry);
            socket.Listen(_maxClientCotunt);
        }

        public void Stop()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Dispose();
        }

        public ServiceStatus GetServiceStatus()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 监听请求连接的客户端
        /// </summary>
        void ListenClient()
        {
            while (true)
            {
                var client = socket.Accept();
                if (client == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                var ipAddress = (client.RemoteEndPoint as IPEndPoint).Address;
                serviceLog?.Invoke($"客户端{ipAddress}已成功连接至{_host}");
            }
        }

        public void Dispose()
        {

        }

        Action<string> serviceLog;
    }
}
