using Server.Udp.MessageProcessing.Handlers;
using Server.Udp.Storage;
using System;
using System.Net;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.GameEngine.MessageSorters;
using Server.GameEngine.Rudp;
using Server.Udp.Sending;

namespace Server.Udp.MessageProcessing
{
    /// <summary>
    /// Перенаправляет все сообщения от игроков по обработчикам.
    /// </summary>
    public class MessageWrapperHandler
    {
        private readonly PingMessageHandler pingMessageHandler;
        private readonly InputMessageHandler inputMessageHandler;
        private readonly PlayerExitMessageHandler exitMessageHandler;
        private readonly RudpConfirmationReceiver rudpConfirmationHandler;

        public MessageWrapperHandler(InputEntitiesCreator inputEntitiesCreator, 
            ExitEntitiesCreator exitEntitiesCreator,
             ByteArrayRudpStorage byteArrayRudpStorage,
             IUdpSender udpSender,
             IpAddressesStorage ipAddressesStorage)
        {
            pingMessageHandler = new PingMessageHandler(ipAddressesStorage, udpSender);
            inputMessageHandler = new InputMessageHandler(inputEntitiesCreator);
            exitMessageHandler = new PlayerExitMessageHandler(exitEntitiesCreator);
            rudpConfirmationHandler = new RudpConfirmationReceiver(byteArrayRudpStorage);
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            // if (messageWrapper.NeedResponse)
            // {
            //     rudpConfirmationSender.Handle(messageWrapper, sender);
            // }

            switch (messageWrapper.MessageType)
            {
                case MessageType.PlayerInputMessagesPack:
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