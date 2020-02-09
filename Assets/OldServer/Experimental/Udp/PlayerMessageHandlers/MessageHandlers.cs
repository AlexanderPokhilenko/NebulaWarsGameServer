using AmoebaBattleServer01.Experimental.Udp.PlayerMessageHandlers;
using NetworkLibrary.NetworkLibrary.Udp;
using System;
using System.Net;

namespace AmoebaBattleServer01.Experimental.PlayerMessageHandlers
{
    internal class MessageHandlers
    {
        private readonly PlayerInputHandler playerInputHandler = new PlayerInputHandler();
        private readonly PlayerPingHandler pingHandler = new PlayerPingHandler();
        
        public void Handle(Message message, IPEndPoint sender)
        {
            switch (message.MessageType)
            {
                case MessageType.PlayerInput:
                    playerInputHandler.Handle(message, sender);
                    break;
                case MessageType.PlayerPing:
                    pingHandler.Handle(message, sender);
                    break;
                default:
                    throw new Exception("Неожиданный тип сообщения "+message.MessageType);
            }
        }
    }
}