﻿using System;
using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using OldServer.Experimental.Udp.MessageProcessing.Handlers;

namespace OldServer.Experimental.Udp.MessageProcessing
{
    internal class MessageProcessor
    {
        private readonly InputMessageHandler inputMessageHandler = new InputMessageHandler();
        private readonly PingMessageHandler pingMessageHandler = new PingMessageHandler();
        private readonly DeliveryConfirmationMessageHandler confirmationMessageHandler = new DeliveryConfirmationMessageHandler();
        private readonly RudpConfirmationSender confirmationSender = new RudpConfirmationSender();
        
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
                case MessageType.DeliveryConfirmationFromClient:
                    confirmationMessageHandler.Handle(message, sender);
                    break;
                default:
                    throw new Exception("Неожиданный тип сообщения "+message.MessageType);
            }
        }
    }
}