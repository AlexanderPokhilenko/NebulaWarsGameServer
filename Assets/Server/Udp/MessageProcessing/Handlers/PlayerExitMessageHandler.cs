using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine;
using Server.GameEngine.Experimental;
using Server.Http;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    //TODO это очень опасно. злоумышленник может исключить игроков из списка активных игроков всех
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitMessageHandler));
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Error("Сообщение о выходе из боя пришло");
            BattleExitMessage exitMessage =
                ZeroFormatterSerializer.Deserialize<BattleExitMessage>(messageWrapper.SerializedMessage);

            if (exitMessage.PlayerId == 0)
            {
                throw new ArgumentOutOfRangeException("exitMessage.PlayerId = "+exitMessage.PlayerId);
            }
            
            StaticExitMessageSorter.AddExitMessage(exitMessage.PlayerId);

            // if (NetworkMediator.IpAddressesStorage.TryRemovePlayerIp(exitMessage.PlayerId))
            // {
            //     Log.Info($"ip игрока с id {exitMessage.PlayerId} удалён");
            // }
            // else
            // {
            //     Log.Warn($"ip игрока с id {exitMessage.PlayerId} не удалён");
            // }

            // if (GameEngineMediator.MatchStorageFacade.TryRemovePlayer(exitMessage.PlayerId))
            // {
            //     Log.Info($"игрока с id {exitMessage.PlayerId} удалён из списка активных игроков");
            // }
            // else
            // {
            //     Log.Warn($"игрока с id {exitMessage.PlayerId} не удалён из списка активных игроков");
            // }
            
        }
    }
}