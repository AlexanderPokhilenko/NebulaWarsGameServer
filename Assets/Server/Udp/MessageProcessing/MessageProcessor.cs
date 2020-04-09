using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using Server.Udp.MessageProcessing.Handlers;

namespace Server.Udp.MessageProcessing
{
    /// <summary>
    /// Перенаправляет все сообщения от игроков по обработчикам.
    /// </summary>
    internal class MessageProcessor
    {
        private readonly InputMessageHandler inputMessageHandler;
        private readonly PlayerExitMessageHandler exitMessageHandler;
        
        private readonly PingMessageHandler pingMessageHandler = new PingMessageHandler();
        private readonly RudpConfirmationReceiver confirmationReceiver = new RudpConfirmationReceiver();
        private readonly RudpConfirmationSender confirmationSender = new RudpConfirmationSender();

        public MessageProcessor(InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator)
        {
            inputMessageHandler = new InputMessageHandler(inputEntitiesCreator);
            exitMessageHandler = new PlayerExitMessageHandler(exitEntitiesCreator);
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            if (messageWrapper.NeedResponse)
            {
                confirmationSender.Handle(messageWrapper, sender);
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
                    confirmationReceiver.Handle(messageWrapper, sender);
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