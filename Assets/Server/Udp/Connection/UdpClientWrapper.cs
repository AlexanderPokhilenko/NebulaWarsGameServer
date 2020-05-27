using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Прослушивает указанный порт. При получении сообщения вызывает свой метод HandleBytes
    /// </summary>
    public class UdpClientWrapper
    {
        private readonly ILog log = LogManager.GetLogger(typeof(UdpClientWrapper));
        private UdpClient udpClient;
        private Thread receiveThread;
        private bool isThreadRunning;
        
        public UdpClientWrapper SetupConnection(int listenPort)
        {
            try
            {
                udpClient = new UdpClient(listenPort)
                {
                    Client =
                    {
                        Blocking = false,
                        ReceiveTimeout = 1000
                    }
                };
            }
            catch (Exception e)
            {
                log.Info("Failed to listen for UDP at port " + listenPort + ": " + e.Message);
            }
            
            log.Info("Создан udp клиент на порте " + listenPort);
            return this;
        }
     
        public void StartReceiveThread()
        {
            if (udpClient != null)
            {
                receiveThread = new Thread(async () => await StartEndlessLoop(udpClient));
                isThreadRunning = true;
                receiveThread.Start();
            }
            else
            {
                throw new Exception("А ну соединение мне запили");
            }
        }
     
        private async Task StartEndlessLoop(UdpClient client)
        {
            while (isThreadRunning)
            {
                try
                {
                    if (client != null)
                    {
                        UdpReceiveResult result = await client.ReceiveAsync();
                        byte[] data = result.Buffer;
                        HandleBytes(data, result.RemoteEndPoint);   
                    }
                }
                catch (SocketException)
                {
                    // 10004 thrown when socket is closed
                    // if (e.ErrorCode != 10004) Log.Info("Socket exception while receiving data from udp client: " + e.MessageWrapper);
                }
                catch (Exception e)
                {
                    log.Info("Error receiving data from udp client: " + e.Message+" "+e.StackTrace);
                }
            }
        }

        public void Send(byte[] data, IPEndPoint endPoint)
        {
            udpClient.Send(data, data.Length, endPoint);
        }
        
        public void Stop()
        {
            log.Info("Остановка udp клиента");
            isThreadRunning = false;
            udpClient.Close();
            receiveThread.Interrupt();
        }
        
        protected virtual void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            // Log.Info($"Пришло сообщение размером в {data.Length} байт");
        }
    }
}