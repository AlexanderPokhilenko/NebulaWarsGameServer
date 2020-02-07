﻿using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental.Udp
{
    public static class UdpSendUtils
    {
        public static void SendPositions(int targetPlayerId, GameEntity[] withPosition)
        {
            // Console.WriteLine("SendPositions ");
            
            var mes = new PositionsMessage()
            {
                EntitiesInfo = new Dictionary<int, ViewTransform>(withPosition.Length)
            };

            foreach (var gameEntity in withPosition)
            {
                //string playerGoogleId = gameEntity.player.GoogleId;
                var gt = gameEntity.globalTransform;
                var typeId = gameEntity.viewType.id;
                var transform = new ViewTransform(gt.position, gt.angle, typeId);
                mes.EntitiesInfo.Add(gameEntity.id.value, transform);
            }
            
            var address = BentMediator.PlayersIpAddressesWrapper.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                Message message = MessageFactory.GetMessage(mes);
                byte[] data = ZeroFormatterSerializer.Serialize(message);
                BentMediator.UdpBattleConnection.Send(data, address);
            }
        }
    }
}