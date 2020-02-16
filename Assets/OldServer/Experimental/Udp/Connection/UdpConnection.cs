using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//TODO возможно стоит убрать метод Close, так как прослушка udp никогда не должна прекращаться при работе сервера 

namespace OldServer.Experimental.Udp.Connection
{
    public class UdpConnection
    {
        Thread receiveThread;
        private UdpClient udpClient;
        private bool isThreadRunning;

        public void SetConnection(int listenPort)
        {
            try
            {
                udpClient = new UdpClient(listenPort);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to listen for UDP at port " + listenPort + ": " + e.Message);
                return;
            }
            
            Console.WriteLine("Создан udp клиент на порте " + listenPort);
        }
     
        public void StartReceiveThread()
        {
            receiveThread = new Thread(() => StartEndlessLoop(udpClient));
            isThreadRunning = true;
            receiveThread.Start();
        }
     
        private void StartEndlessLoop(UdpClient client)
        {
            while (isThreadRunning)
            {
                IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    //Ждёт udp сообщения
                    byte[] data = client.Receive(ref remoteIpEndPoint);
                    HandleBytes(data, remoteIpEndPoint);
                }
                catch (SocketException e)
                {
                    // 10004 thrown when socket is closed
                    // if (e.ErrorCode != 10004) Console.WriteLine("Socket exception while receiving data from udp client: " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error receiving data from udp client: " + e.Message);
                }
            }
        }

        public void Send(byte[] data, IPEndPoint endPoint)
        {
            // Console.WriteLine($"Отправка сообщения на {endPoint.Address} {endPoint.Port} размером в {data.Length} байтов");
            udpClient.Send(data, data.Length, endPoint);
        }
     
        public void Stop()
        {
            Console.WriteLine("Остановка udp клиента");
            isThreadRunning = false;
            receiveThread.Interrupt();
            udpClient.Close();
        }

        /// <summary>
        /// Обработка нового сообщения
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endPoint"></param>
        protected virtual void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            // Console.WriteLine($"Пришло сообщение размером в {data.Length} байт");
        }
    }
}