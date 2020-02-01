using System;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental.Udp
{
    public static class UdpSendUtils
    {
        public static void SendPositions(string targetPlayerGoogleId, GameEntity[] withPosition)
        {
            // Console.WriteLine("SendPositions ");
            
            PlayersPositionsMessage mes = new PlayersPositionsMessage();
            mes.PlayersInfo = new Dictionary<string, Vector2>();

            foreach (var gameEntity in withPosition)
            {
                string playerGoogleId = gameEntity.player.PlayerGoogleId;
                Vector2 position = new Vector2(gameEntity.position.X, gameEntity.position.Y); 
                mes.PlayersInfo.Add(playerGoogleId, position);
            }
            
            var address = BentMediator.PlayersIpAddressesWrapper.GetPlayerIpAddress(targetPlayerGoogleId);
            if (address != null)
            {
                Message message = MessageFactory.GetMessage(mes);
                byte[] data = ZeroFormatterSerializer.Serialize(message);
                BentMediator.UdpBattleConnection.Send(data, address);
            }
        }
    }
}