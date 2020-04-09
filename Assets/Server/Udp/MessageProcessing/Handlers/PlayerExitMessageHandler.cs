using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    //TODO это очень опасно. злоумышленник может исключить игроков из списка активных игроков всех
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitMessageHandler));
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            Log.Warn("Сообщение о выходе из боя пришло");
            BattleExitMessage exitMessage =
                ZeroFormatterSerializer.Deserialize<BattleExitMessage>(messageWrapper.SerializedMessage);

            if (exitMessage.PlayerId == 0)
            {
                throw new ArgumentOutOfRangeException("exitMessage.PlayerId = "+exitMessage.PlayerId);
            }
            
            ExitEntitiesCreator.AddExitMessage(exitMessage.PlayerId);
        }
    }
}