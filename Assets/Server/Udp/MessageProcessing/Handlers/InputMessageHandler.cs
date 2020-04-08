using System.Net;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using Server.GameEngine.Experimental;
using ZeroFormatter;

namespace Server.Udp.MessageProcessing.Handlers
{
    public class InputMessageHandler:IMessageHandler
    {
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
            StaticInputMessagesSorter.TryAddMovementMessage(playerId, vector);
        }

        private void AddPlayerAttackComponent(int playerId, float angle)
        {
            StaticInputMessagesSorter.TryAddAttackMessage(playerId, angle);
        }

        private void AddPlayerAbilityComponent(int playerId, bool ability)
        {
            StaticInputMessagesSorter.TryAddAbilityMessage(playerId, ability);
        }
    }
}