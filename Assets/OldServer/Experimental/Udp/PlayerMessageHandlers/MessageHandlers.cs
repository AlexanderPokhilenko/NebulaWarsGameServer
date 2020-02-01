using System;
using System.Net;
using AmoebaBattleServer01.Experimental.Udp.PlayerMessageHandlers;
using NetworkLibrary;
using NetworkLibrary.NetworkLibrary.Udp;

namespace AmoebaBattleServer01.Experimental.PlayerMessageHandlers
{
    internal class MessageHandlers
    {
        private readonly PlayerJoystickInputHandler playerJoystickInputHandler = new PlayerJoystickInputHandler();
        private readonly PlayerPingHandler pingHandler = new PlayerPingHandler();
        
        public void Handle(Message message, IPEndPoint sender)
        {
            switch (message.MessageType)
            {
                case 3:
                    playerJoystickInputHandler.Handle(message, sender);
                    break;
                case 6:
                    pingHandler.Handle(message, sender);
                    break;
                default:
                    throw new Exception("Неожиданный тип сообщения "+message.MessageType);
            }
        }
    }
}