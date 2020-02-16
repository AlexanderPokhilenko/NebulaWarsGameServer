using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.Connection;
using OldServer.Experimental.Udp.MessageProcessing;
using OldServer.Experimental.Udp.Storage;
using ZeroFormatter;

namespace OldServer.Experimental.Udp
{
    public class NetworkMediator
    {
        public static UdpBattleConnection udpBattleConnection;
        public static readonly PlayersIpAddressesStorage PlayersIpAddressesStorage = new PlayersIpAddressesStorage();
        private static readonly MessageProcessor MessageProcessor = new MessageProcessor();
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            Message message = ZeroFormatterSerializer.Deserialize<Message>(data);
            MessageProcessor.Handle(message, endPoint);
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