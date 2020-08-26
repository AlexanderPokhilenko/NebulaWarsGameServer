using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Plugins.submodules.SharedCode.Logger;
using Server.Udp.Sending;

namespace Server.Udp.Connection
{
    /// <summary>
    /// Прослушивает указанный порт. При получении сообщения вызывает свой метод HandleBytes.
    /// Может отправлять дейтаграммы.
    /// </summary>
    public class UdpClientWrapper:IUdpSender
    {
        private UdpClient udpClient;
        private Thread receiveThread;
        private bool isThreadRunning;
        private readonly ILog log = LogManager.CreateLogger(typeof(UdpClientWrapper));
        
        public UdpClientWrapper SetupConnection(int listenPort)
        {
            try
            {
                udpClient = new UdpClient(listenPort)
                {
                    Client =
                    {
                        Blocking = false
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
     
        public void StartReceiveThread(IByteArrayHandler byteArrayHandler)
        {
            if (udpClient != null)
            {
                receiveThread = new Thread(async () => await StartEndlessLoop(udpClient, byteArrayHandler));
                isThreadRunning = true;
                receiveThread.Start();
            }
            else
            {
                throw new Exception("А ну соединение мне запили");
            }
        }
     
        private async Task StartEndlessLoop(UdpClient client, IByteArrayHandler handler)
        {
            while (isThreadRunning)
            {
                try
                {
                    if (client != null)
                    {
                        UdpReceiveResult result = await client.ReceiveAsync();
                        byte[] data = result.Buffer;
                        handler.HandleBytes(data, result.RemoteEndPoint);   
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
            receiveThread.Join();
        }
    }
}