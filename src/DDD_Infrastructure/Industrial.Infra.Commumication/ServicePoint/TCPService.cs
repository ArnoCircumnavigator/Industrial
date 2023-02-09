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
        readonly int _port = 0;
        readonly int _maxClientCotunt = 0;
        ServiceStatus status;
        Socket socket;
        public TCPService(int port, int maxClientCount)
        {
            _port = port;
            _maxClientCotunt = maxClientCount;
        }

        public void Start()
        {
            status = ServiceStatus.Starting;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var hostEntry = new IPEndPoint(IPAddress.Any, _port);
            socket.Bind(hostEntry);
            socket.Listen(_maxClientCotunt);
            status = ServiceStatus.Runing;
            Thread thread = new Thread(new ParameterizedThreadStart((_) =>
            {
                ListenClient();
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Dispose();
        }

        public ServiceStatus GetServiceStatus()
        {
            return status;
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
                //客户端
                var client = socket.Accept();
                if (client == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                var ipAddress = (client.RemoteEndPoint as IPEndPoint).Address;

                serviceLog?.Invoke($"客户端{ipAddress}已成功连接");
                

                var result = ThreadPool.QueueUserWorkItem(ServerRecMsg, client);
                if (!result)
                {
                    serviceLog?.Invoke($"无法开启正确Accept!");
                }



            }
        }
        void ServerRecMsg(object obj)
        {
            var socket = (Socket)obj;

        }

        public void Dispose()
        {
            status = ServiceStatus.Disposing;
            Stop();
            status = ServiceStatus.Disposed;
        }

        Action<string> serviceLog;
    }
}
