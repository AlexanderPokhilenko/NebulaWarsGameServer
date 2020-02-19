using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.Utils;
using UnityEngine;

namespace Server.Udp.Connection
{
    public class UdpConnection
    {
        Thread receiveThread;
        private UdpClient udpClient;
        private bool isThreadRunning;

        public UdpConnection SetConnection(int listenPort)
        {
            try
            {
                udpClient = new UdpClient(listenPort);
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
                receiveThread = new Thread(() => StartEndlessLoop(udpClient));
                isThreadRunning = true;
                receiveThread.Start();    
            }
            else
            {
                throw new Exception("А ну соединение мне запили");
            }
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
            udpClient.Send(data, data.Length, endPoint);
        }
     
        public void Stop()
        {
            Log.Info("Остановка udp клиента");
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
            // Log.Info($"Пришло сообщение размером в {data.Length} байт");
        }
    }
}