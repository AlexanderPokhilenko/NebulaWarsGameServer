using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Libraries.NetworkLibrary.Udp.Common;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.Udp.Storage;
using Server.Utils;
using ZeroFormatter;

//TODO некрасиво

namespace Server.Udp.Sending
{
    public static class UdpSendUtils
    {
        public static void SendPositions(int targetPlayerId, IEnumerable<GameEntity> withPosition)
        {
            var gameEntities = withPosition.ToList();
            var message = new PositionsMessage
            {
                EntitiesInfo = new Dictionary<int, ViewTransform>(gameEntities.Count),
                //TODO: перенести установление в UDP с подтверждением
                PlayerEntityId = gameEntities.First(entity => entity.hasPlayer && entity.player.id == targetPlayerId)
                    .id.value
            };

            foreach (var gameEntity in gameEntities)
            {
                var gt = gameEntity.globalTransform;
                var typeId = gameEntity.viewType.id;
                var transform = new ViewTransform(gt.position, gt.angle, typeId);
                message.EntitiesInfo.Add(gameEntity.id.value, transform);
            }
            
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId); 
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
        }

        public static void SendMessage(MessageWrapper messageWrapper, int playerId)
        {
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(playerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(messageWrapper);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
            else
            {
                throw new Exception("Не удаётся отправить udp сообщение так как не известен ip этого игрока "+playerId);
            }
        }
        
        public static void SendMessage(byte[] serializedMessage, int playerId)
        {
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(playerId);
            if (address != null)
            {
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
            else
            {
                throw new Exception("Не удаётся отправить udp сообщение так как не известен ip этого игрока "+playerId);
            }
        }
        
        public static void SendDeliveryConfirmationMessage(DeliveryConfirmationMessage message, IPEndPoint address)
        {
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message, false, out uint messageId);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
            else
            {
                throw new Exception("SendDeliveryConfirmationMessage address == null");
            }
        }

        public static void SendHp(int targetPlayerId, float healthPoints)
        {
            var address = NetworkMediator.IpAddressesStorage.GetPlayerIpAddress(targetPlayerId);
            HealthPointsMessage healthPointsMessage = new HealthPointsMessage(healthPoints);
            if (address != null)
            {
                Log.Warning($"Отправка хп игрока {targetPlayerId} {healthPoints}");
                var serializedMessage =
                    MessageFactory.GetSerializedMessage(healthPointsMessage, true, out uint messageId);
                ByteArrayRudpStorage.Instance.AddMessage(targetPlayerId,  messageId, serializedMessage);
                NetworkMediator.udpBattleConnection.Send(serializedMessage, address);
            }
        }
    }
}