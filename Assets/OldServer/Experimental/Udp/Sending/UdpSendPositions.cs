using System;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;

namespace OldServer.Experimental.Udp.Sending
{
    public static partial class UdpSendUtils
    {
        public static void SendPositions(int targetPlayerId, GameEntity[] withPosition)
        {
            var mes = new PositionsMessage
            {
                EntitiesInfo = new Dictionary<int, ViewTransform>(withPosition.Length),
                //TODO: перенести установление в UDP с подтверждением
                PlayerEntityId = withPosition.First(entity => entity.hasPlayer && entity.player.id == targetPlayerId)
                    .id.value
            };

            foreach (var gameEntity in withPosition)
            {
                var gt = gameEntity.globalTransform;
                var typeId = gameEntity.viewType.id;
                var transform = new ViewTransform(gt.position, gt.angle, typeId);
                mes.EntitiesInfo.Add(gameEntity.id.value, transform);
            }
            
            var address = NetworkMediator.PlayersIpAddressesStorage.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(mes);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
        }

        public static void SendMessage(Message message, int playerId)
        {
            var address = NetworkMediator.PlayersIpAddressesStorage.GetPlayerIpAddress(playerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(message);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
            else
            {
                throw new Exception("Не удаётся отправить udp сообщение так как не известен ip этого игрока "+playerId);
            }
        }
    }
}