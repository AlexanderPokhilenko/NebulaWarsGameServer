using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental.Udp
{
    public static class UdpSendUtils
    {
        public static void SendPositions(int targetPlayerId, IEnumerable<GameEntity> viewObjects)
        {
            // Console.WriteLine("SendPositions ");
            
            var mes = new PositionsMessage()
            {
                EntitiesInfo = viewObjects.ToDictionary(e => e.id.value,
                    e => new ViewTransform(e.globalTransform.position, e.globalTransform.angle, e.viewType.id)),
                //TODO: перенести в UDP с подтверждением
                PlayerEntityId = viewObjects.First(entity => entity.hasPlayer && entity.player.id == targetPlayerId).id.value
            };
            
            var address = BentMediator.PlayersIpAddressesWrapper.GetPlayerIpAddress(targetPlayerId);
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(mes);
                BentMediator.UdpBattleConnection.Send(data, address);
            }
        }
    }
}