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
                var transform = Transform.GetTransform(gameEntity);
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