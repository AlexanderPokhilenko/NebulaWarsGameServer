using System.Net;
using AmoebaBattleServer01.Experimental.GameEngine;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

namespace OldServer.Experimental.Udp.PlayerMessageHandlers
{
    public class PlayerInputHandler:IMessageHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            PlayerInputMessage mes =
                ZeroFormatterSerializer.Deserialize<PlayerInputMessage>(message.SerializedMessage);
            
            AddPlayerInputComponent(mes.TemporaryIdentifier, mes.GetVector2());
            AddPlayerAttackComponent(mes.TemporaryIdentifier, mes.Angle);
        }

        private void AddPlayerInputComponent(int playerId, Vector2 vector)
        {
            StaticInputMessagesSorter.MovementMessages.TryAdd(playerId, vector);
        }

        private void AddPlayerAttackComponent(int playerId, float angle)
        {
            StaticInputMessagesSorter.AttackMessages.TryAdd(playerId, angle);
        }
    }
}