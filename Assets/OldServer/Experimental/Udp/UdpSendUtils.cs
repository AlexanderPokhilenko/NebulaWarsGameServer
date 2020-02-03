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
            
            PositionsMessage mes = new PositionsMessage();

            foreach (var gameEntity in withPosition)
            {
                //string playerGoogleId = gameEntity.player.GoogleId;

                var position = gameEntity.position;
                var direction = gameEntity.direction.angle;
                var transform = new Transform(position.value.x, position.value.y, direction);
                mes.EntitiesInfo.Add(gameEntity.id.value, transform); //TODO: нужно отправлять глобальные позиции!!!
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