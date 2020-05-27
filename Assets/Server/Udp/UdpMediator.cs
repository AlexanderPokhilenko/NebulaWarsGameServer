using System.Net;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Udp.MessageProcessing;
using Server.Udp.Sending;
using Server.Udp.Storage;
using ZeroFormatter;

namespace Server.Udp
{
    public class UdpMediator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(UdpMediator));
        
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
    }
}