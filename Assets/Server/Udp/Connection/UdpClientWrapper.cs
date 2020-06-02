using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Code.Common;
using Server.Udp.Sending;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Прослушивает указанный порт. При получении сообщения вызывает свой метод HandleBytes.
    /// Может отправлять дейтаграммы.
    /// </summary>
    public abstract class UdpClientWrapper:IUdpSender
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(UdpClientWrapper));
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
                    // if (e.ErrorCode != 10004) Log.Info("Socket exception while receiving serializedContainer from udp client: " + e.MessageWrapper);
                }
                catch (Exception e)
                {
                    log.Info("Error receiving serializedContainer from udp client: " + e.Message+" "+e.StackTrace);
                }
            }
        }
        
        protected virtual void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
        }

        public void Send(byte[] serializedContainer, IPEndPoint endPoint)
        {
            int lengthOfTheSentDatagram = udpClient.Send(serializedContainer, serializedContainer.Length, endPoint);
            if (lengthOfTheSentDatagram != serializedContainer.Length)
            {
                log.Warn("Ошибка длины сообщения.");
            }
        }
        
        public void Stop()
        {
            log.Info("Остановка udp клиента");
            isThreadRunning = false;
            udpClient.Close();
            receiveThread.Interrupt();
            receiveThread.Join();
        }
    }
}