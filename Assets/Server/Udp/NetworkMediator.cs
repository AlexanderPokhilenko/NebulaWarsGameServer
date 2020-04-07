﻿using System;
using System.Net;
using System.Threading;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp
{
    //TODO говно
    public class NetworkMediator
    {
        private static readonly MessageProcessor MessageProcessor = new MessageProcessor();
        
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
                udpBattleConnection.SetMediator(this);
            }
            else
            {
                throw new Exception("Медиатор уже содержит ссылку на udp соединение");
            }
        }
    }
}