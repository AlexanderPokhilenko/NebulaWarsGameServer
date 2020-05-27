using System;
using System.Net;
using System.Net.Sockets;
using Libraries.NetworkLibrary.Udp.Common;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Udp.MessageProcessing.Handlers;
using Server.Udp.Sending;
using Server.Udp.Storage;
using UnityEngine;
using ZeroFormatter;

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
            MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, UdpSendUtils udpSendUtils)
        {
            inputMessageHandler = new InputMessageHandler(inputEntitiesCreator);
            exitMessageHandler = new PlayerExitMessageHandler(exitEntitiesCreator);
            pingMessageHandler = new PingMessageHandler(matchStorage);
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
                case MessageType.Debug:
                    break;
                //     UdpClient udpClient = new UdpClient();
                //     DebugMessage debugMessage = new DebugMessage();
                //     byte[] data = ZeroFormatterSerializer.Serialize(debugMessage);
                //     udpClient.Send(data, data.Length, sender);
                //     return;
                default:
                    throw new Exception("Неожиданный тип сообщения "+messageWrapper.MessageType);
            }
        }
    }
}