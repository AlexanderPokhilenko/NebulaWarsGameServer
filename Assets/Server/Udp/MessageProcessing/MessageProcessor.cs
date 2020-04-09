using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
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
        
        private readonly PingMessageHandler pingMessageHandler;
        private readonly RudpConfirmationReceiver rudpConfirmationHandler;
        private readonly RudpConfirmationSender rudpConfirmationSender = new RudpConfirmationSender();

        public MessageProcessor(InputEntitiesCreator inputEntitiesCreator, ExitEntitiesCreator exitEntitiesCreator,
            MatchStorage matchStorage)
        {
            inputMessageHandler = new InputMessageHandler(inputEntitiesCreator);
            exitMessageHandler = new PlayerExitMessageHandler(exitEntitiesCreator);
            pingMessageHandler = new PingMessageHandler(matchStorage);
            rudpConfirmationHandler = new RudpConfirmationReceiver(matchStorage);
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