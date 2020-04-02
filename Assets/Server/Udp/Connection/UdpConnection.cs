using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Server.Udp.Connection
{
    public class UdpConnection
    {
        private Thread receiveThread;
        private UdpClient udpClient;
        private bool isThreadRunning;

        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpConnection));
        
        public UdpConnection SetUpConnection(int listenPort)
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
                Log.Info("Failed to listen for UDP at port " + listenPort + ": " + e.Message);
            }
            
            Log.Info("Создан udp клиент на порте " + listenPort);
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
                    var result = await client.ReceiveAsync();
                    byte[] data = result.Buffer;
                    HandleBytes(data, result.RemoteEndPoint);
                }
                catch (SocketException)
                {
                    // 10004 thrown when socket is closed
                    // if (e.ErrorCode != 10004) Log.Info("Socket exception while receiving data from udp client: " + e.MessageWrapper);
                }
                catch (Exception e)
                {
                    Log.Info("Error receiving data from udp client: " + e.Message);
                }
            }
        }

        public void Send(byte[] data, IPEndPoint endPoint)
        {
            // Log.Info($"Отправка сообщения на {endPoint.Address} {endPoint.Port} размером в {data.Length} байтов");
            try
            {
                udpClient.Send(data, data.Length, endPoint);
            }
            catch (SocketException)
            {
                //ignore   
            }
        }
     
        public void Stop()
        {
            Log.Info("Остановка udp клиента");
            isThreadRunning = false;
            receiveThread.Interrupt();
            udpClient.Close();
        }
        
        protected virtual void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            // Log.Info($"Пришло сообщение размером в {data.Length} байт");
        }
    }
}