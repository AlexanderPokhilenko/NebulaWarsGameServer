using System;
using System.Net;
using Code.Common;
using Libraries.Logger;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using Server.GameEngine.MessageSorters;
using ZeroFormatter;

//TODO это очень опасно. злоумышленник может исключить всех игроков из списка активных игроков и им перестанет
//отправляться инфа про матч
//TODO нужно добавить секрет для защиты

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.CreateLogger(typeof(PlayerExitMessageHandler));
        
        private readonly ExitEntitiesCreator exitEntitiesCreator;

        public PlayerExitMessageHandler(ExitEntitiesCreator exitEntitiesCreator)
        {
            this.exitEntitiesCreator = exitEntitiesCreator;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Warn("Сообщение о выходе из боя пришло");
            BattleExitMessage exitMessage =
                ZeroFormatterSerializer.Deserialize<BattleExitMessage>(messageWrapper.SerializedMessage);

            if (exitMessage.TemporaryId == 0)
            {
                throw new ArgumentOutOfRangeException("exitMessage.TemporaryId = "+exitMessage.TemporaryId);
            }
            
            exitEntitiesCreator.AddExitMessage(exitMessage.MatchId, exitMessage.TemporaryId);
        }
    }
}