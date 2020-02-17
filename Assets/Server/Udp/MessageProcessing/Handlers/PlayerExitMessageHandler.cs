using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using NetworkLibrary.NetworkLibrary.Udp;
using UnityEngine;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PlayerExitMessageHandler:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            BattleExitMessage exitMessage =
                ZeroFormatterSerializer.Deserialize<BattleExitMessage>(message.SerializedMessage);
            
            if(exitMessage.PlayerId==0) throw new ArgumentOutOfRangeException("exitMessage.PlayerId = "+exitMessage.PlayerId);

            if (NetworkMediator.IpAddressesStorage.TryRemovePlayerIp(exitMessage.PlayerId))
            {
                Debug.LogWarning($"Игрок с id {exitMessage.PlayerId} удалён из списка");
            }
        }
    }
}