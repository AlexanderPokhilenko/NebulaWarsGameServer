﻿using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using ZeroFormatter;

namespace Server.Udp
{
    public class UdpMediator
    {
        public static UdpConnectionFacade udpConnectionFacade { get; private set; }

        private readonly MessageProcessor messageProcessor;

        public UdpMediator(InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator)
        {
            messageProcessor = new MessageProcessor(inputEntitiesCreator, exitEntitiesCreator);
        }
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageProcessor.Handle(messageWrapper, endPoint);
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