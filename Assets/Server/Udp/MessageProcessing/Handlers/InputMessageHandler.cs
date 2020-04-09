using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine.Experimental;
using ZeroFormatter;

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
            PlayerInputMessage mes =
                ZeroFormatterSerializer.Deserialize<PlayerInputMessage>(messageWrapper.SerializedMessage);
            
            AddPlayerInputComponent(mes.TemporaryIdentifier, mes.GetVector2());
            AddPlayerAttackComponent(mes.TemporaryIdentifier, mes.Angle);
            AddPlayerAbilityComponent(mes.TemporaryIdentifier, mes.UseAbility);
        }

        private void AddPlayerInputComponent(int playerId, Vector2 vector)
        {
            if (inputEntitiesCreator.TryAddMovementMessage(playerId, vector))
            {
                //намана
            }
            else
            {
                //шо?
            }
        }

        private void AddPlayerAttackComponent(int playerId, float angle)
        {
            if (inputEntitiesCreator.TryAddAttackMessage(playerId, angle))
            {
                //намана
            }
            else
            {
                //шо?
            }
        }

        private void AddPlayerAbilityComponent(int playerId, bool ability)
        {
            inputEntitiesCreator.TryAddAbilityMessage(playerId, ability);
        }
    }
}