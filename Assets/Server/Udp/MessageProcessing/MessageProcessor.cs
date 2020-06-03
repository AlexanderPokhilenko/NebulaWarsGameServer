using Libraries.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using Server.Udp.MessageProcessing.Handlers;
using Server.Udp.Sending;
using Server.Udp.Storage;
using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;

namespace Server.Udp.MessageProcessing
{
    /// <summary>
    /// Перенаправляет все сообщения от игроков по обработчикам.
    /// </summary>
    public class MessageProcessor
    {
        private readonly InputMessageHandler inputMessageHandler;
        private readonly PlayerExitMessageHandler exitMessageHandler;
        
        private readonly PingMessageHandler pingMessageHandler;
        private readonly RudpConfirmationReceiver rudpConfirmationHandler;
        private readonly RudpConfirmationSender rudpConfirmationSender;

        public MessageProcessor(InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator,
             ByteArrayRudpStorage byteArrayRudpStorage, UdpSendUtils udpSendUtils, IpAddressesStorage ipAddressesStorage)
        {
            inputMessageHandler = new InputMessageHandler(inputEntitiesCreator);
            exitMessageHandler = new PlayerExitMessageHandler(exitEntitiesCreator);
            pingMessageHandler = new PingMessageHandler(ipAddressesStorage);
            rudpConfirmationHandler = new RudpConfirmationReceiver(byteArrayRudpStorage);
            rudpConfirmationSender = new RudpConfirmationSender(udpSendUtils);
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            if (messageWrapper.NeedResponse)
            {
                rudpConfirmationSender.Handle(messageWrapper, sender);
            }

            switch (messageWrapper.MessageType)
            {
                case MessageType.PlayerInput:
                    inputMessageHandler.Handle(messageWrapper, sender);
                    break;
                case MessageType.PlayerPing:
                    pingMessageHandler.Handle(messageWrapper, sender);
                    break;
                case MessageType.DeliveryConfirmation:
                    rudpConfirmationHandler.Handle(messageWrapper, sender);
                    break;
                case MessageType.PlayerExit:
                    exitMessageHandler.Handle(messageWrapper, sender);
                    break;
                default:
                    throw new Exception("Неожиданный тип сообщения "+messageWrapper.MessageType);
            }
        }
    }
}