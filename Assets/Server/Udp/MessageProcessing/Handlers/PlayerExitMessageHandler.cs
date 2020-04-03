using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.PlayerToServer;
using log4net;
using NetworkLibrary.NetworkLibrary.Udp;
using Server.Http;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class PlayerExitMessageHandler:IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitMessageHandler));
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            BattleExitMessage exitMessage =
                ZeroFormatterSerializer.Deserialize<BattleExitMessage>(messageWrapper.SerializedMessage);
            
            if(exitMessage.PlayerId==0) 
                throw new ArgumentOutOfRangeException("exitMessage.PlayerId = "+exitMessage.PlayerId);

            if (NetworkMediator.IpAddressesStorage.TryRemovePlayerIp(exitMessage.PlayerId))
            {
                Log.Warn($"ip игрока с id {exitMessage.PlayerId} удалён из списка");
            }
        }
    }
}