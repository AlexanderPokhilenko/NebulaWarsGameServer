using System;
using System.Net;
using Plugins.submodules.SharedCode.Logger;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.PlayerToServer;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.GameEngine.MessageSorters;
using ZeroFormatter;

//TODO это очень опасно. злоумышленник может исключить всех игроков из списка активных игроков и им перестанет
//отправляться инфа про матч
//TODO нужно добавить секрет для защиты

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private readonly ExitEntitiesCreator exitEntitiesCreator;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerExitMessageHandler));

        public PlayerExitMessageHandler(ExitEntitiesCreator exitEntitiesCreator)
        {
            this.exitEntitiesCreator = exitEntitiesCreator;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            log.Warn("Сообщение о выходе из боя пришло");
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