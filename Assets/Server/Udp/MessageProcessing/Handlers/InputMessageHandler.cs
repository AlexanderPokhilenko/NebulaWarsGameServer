using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using Server.GameEngine.Experimental;
using UnityEngine;
using ZeroFormatter;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

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
            PlayerInputMessage message =
                ZeroFormatterSerializer.Deserialize<PlayerInputMessage>(messageWrapper.SerializedMessage);
            int matchId = message.MatchId;
            inputEntitiesCreator.TryAddMovementMessage(matchId, message.TemporaryId, message.GetVector2());
            inputEntitiesCreator.TryAddAttackMessage(matchId, message.TemporaryId, message.Angle);
            inputEntitiesCreator.TryAddAbilityMessage(matchId, message.TemporaryId, message.UseAbility);
        }
    }
}