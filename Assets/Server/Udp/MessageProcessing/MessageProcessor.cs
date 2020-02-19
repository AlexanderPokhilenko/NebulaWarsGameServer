using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Udp.MessageProcessing.Handlers;

namespace Server.Udp.MessageProcessing
{
    internal class MessageProcessor
    {
        private readonly InputMessageHandler inputMessageHandler = new InputMessageHandler();
        private readonly PingMessageHandler pingMessageHandler = new PingMessageHandler();
        private readonly DeliveryConfirmationMessageHandler confirmationMessageHandler = new DeliveryConfirmationMessageHandler();
        private readonly RudpConfirmationSender confirmationSender = new RudpConfirmationSender();
        private readonly PlayerExitMessageHandler exitMessageHandler = new PlayerExitMessageHandler();
        
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            if (messageWrapper.NeedResponse) confirmationSender.Handle(messageWrapper, sender);
            
            switch (messageWrapper.MessageType)
            {
                case MessageType.PlayerInput:
                    inputMessageHandler.Handle(messageWrapper, sender);
                    break;
                case MessageType.PlayerPing:
                    pingMessageHandler.Handle(messageWrapper, sender);
                    break;
                case MessageType.DeliveryConfirmation:
                    confirmationMessageHandler.Handle(messageWrapper, sender);
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