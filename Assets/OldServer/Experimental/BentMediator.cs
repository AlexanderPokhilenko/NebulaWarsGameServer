using System;
using System.Net;
using AmoebaBattleServer01.Experimental.PlayerMessageHandlers;
using AmoebaBattleServer01.Experimental.Udp;
using NetworkLibrary.NetworkLibrary.Udp;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental
{
    public class BentMediator
    {
        public static UdpBattleConnection UdpBattleConnection;
        public static readonly PlayersIpAddressesWrapper PlayersIpAddressesWrapper = new PlayersIpAddressesWrapper();
        private static readonly MessageHandlers MessageHandlers = new MessageHandlers();

        // Вызывается при получения сообщения от udp соединения
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            Message message = ZeroFormatterSerializer.Deserialize<Message>(data);
            MessageHandlers.Handle(message, endPoint);
        }

        public void SetUdpConnection(UdpBattleConnection udpBattleConnection)
        {
            if (UdpBattleConnection == null)
            {
                UdpBattleConnection = udpBattleConnection;
            }
            else
            {
                throw new Exception("Медиатор уже содержит ссылку на udp соединение");
            }
        }
    }
}