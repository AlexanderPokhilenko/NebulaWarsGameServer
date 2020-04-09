﻿using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.GameEngine.Experimental;
using ZeroFormatter;

//TODO это очень опасно. злоумышленник может исключить всех игроков из списка активных игроков и им перестанет
//отправляться инфа про матч

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitMessageHandler));
        
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

            if (exitMessage.PlayerId == 0)
            {
                throw new ArgumentOutOfRangeException("exitMessage.PlayerId = "+exitMessage.PlayerId);
            }
            
            exitEntitiesCreator.AddExitMessage(exitMessage.PlayerId);
        }
    }
}