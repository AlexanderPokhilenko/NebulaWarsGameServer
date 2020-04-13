using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Udp.Connection;
using Server.Udp.MessageProcessing;
using Server.Udp.Sending;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp
{
    public class UdpMediator
    {
        public static UdpListenerFacade udpListenerFacade { get; private set; }

        private readonly MessageProcessor messageProcessor;

        public UdpMediator(InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator,
            MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, UdpSendUtils udpSendUtils)
        {
            messageProcessor = new MessageProcessor(inputEntitiesCreator, exitEntitiesCreator, matchStorage,
                byteArrayRudpStorage, udpSendUtils);
        }
        
        public void HandleBytes(byte[] data, IPEndPoint endPoint)
        {
            MessageWrapper messageWrapper = ZeroFormatterSerializer.Deserialize<MessageWrapper>(data);
            messageProcessor.Handle(messageWrapper, endPoint);
        }

        public void SetUdpConnection(UdpListenerFacade udpConn)
        {
            if (udpListenerFacade == null)
            {
                udpListenerFacade = udpConn;
                udpListenerFacade.SetMediator(this);
            }
            else
            {
                throw new Exception("Медиатор уже содержит ссылку на udp соединение");
            }
        }
    }
}