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
        
        public void Handle(Message message, IPEndPoint sender)
        {
            if (message.NeedResponse) confirmationSender.Handle(message, sender);
            
            switch (message.MessageType)
            {
                case MessageType.PlayerInput:
                    inputMessageHandler.Handle(message, sender);
                    break;
                case MessageType.PlayerPing:
                    pingMessageHandler.Handle(message, sender);
                    break;
                case MessageType.DeliveryConfirmation:
                    confirmationMessageHandler.Handle(message, sender);
                    break;
                case MessageType.PlayerExit:
                    exitMessageHandler.Handle(message, sender);
                    break;
                default:
                    throw new Exception("Неожиданный тип сообщения "+message.MessageType);
            }
        }
    }
}