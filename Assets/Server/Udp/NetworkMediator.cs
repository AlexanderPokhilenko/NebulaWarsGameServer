using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp
{
    public class NetworkMediator
    {
        private static readonly MessageProcessor MessageProcessor = new MessageProcessor();
        //TODO Почему эта херня публичная?
        public static readonly IpAddressesStorage IpAddressesStorage = new IpAddressesStorage();
        public static UdpBattleConnection udpBattleConnection { get; private set; }
        
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            MessageProcessor.Handle(messageWrapper, endPoint);
        }

        public void SetUdpConnection(UdpBattleConnection udpBattleConn)
        {
            if (udpBattleConnection == null)
            {
                udpBattleConnection = udpBattleConn;
            }
            else
            {
                throw new Exception("Медиатор уже содержит ссылку на udp соединение");
            }
        }
    }
}