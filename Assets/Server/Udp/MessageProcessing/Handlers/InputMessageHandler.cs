using System.Net;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.PlayerToServer;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.Utils;
using Server.GameEngine.Experimental;
using Server.GameEngine.MessageSorters;
using UnityEngine;
using UnityEngine.Playables;
using ZeroFormatter;
using Vector2 = Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace Server.Udp.MessageProcessing.Handlers
{
    /// <summary>
    /// Добавляет данные о вводе игрока в очередь.
    /// </summary>
    public class InputMessageHandler:IMessageHandler
    {
        private readonly InputEntitiesCreator inputEntitiesCreator;

        public InputMessageHandler(InputEntitiesCreator inputEntitiesCreator)
        {
            this.inputEntitiesCreator = inputEntitiesCreator;
        }
        
        public void Handle(MessageWrapper messageWrapper, IPEndPoint sender)
        {
            InputMessagesPack message =
                ZeroFormatterSerializer.Deserialize<InputMessagesPack>(messageWrapper.SerializedMessage);
            //todo понять какие сообщения больше не нужны
            inputEntitiesCreator.AddInputMessage(message);
        }
    }
}