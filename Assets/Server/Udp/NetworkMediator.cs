using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using ZeroFormatter;

namespace Server.Udp
{
    public class NetworkMediator
    {
        private static readonly MessageProcessor MessageProcessor = new MessageProcessor();
        //TODO говно
        public static UdpConnectionFacade udpConnectionFacade { get; private set; }
        
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            MessageProcessor.Handle(messageWrapper, endPoint);
        }

        public void SetUdpConnection(UdpConnectionFacade udpConn)
        {
            if (udpConnectionFacade == null)
            {
                udpConnectionFacade = udpConn;
                udpConnectionFacade.SetMediator(this);
            }
            else
            {
                throw new Exception("Медиатор уже содержит ссылку на udp соединение");
            }
        }
    }
}