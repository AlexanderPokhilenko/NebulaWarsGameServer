using System.Net;
using AmoebaBattleServer01.Experimental.GameEngine;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

namespace AmoebaBattleServer01.Experimental.PlayerMessageHandlers
{
    public class PlayerJoystickInputHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            PlayerJoystickInputMessage mes =
                ZeroFormatterSerializer.Deserialize<PlayerJoystickInputMessage>(message.SerializedMessage);
            
            AddPlayerInputComponent(mes.PlayerGoogleId, mes.GetVector2());
        }

        private void AddPlayerInputComponent(string playerGoogleId, Vector2 vector)
        {
            StaticInputMessagesSorter.MovementMessages.TryAdd(playerGoogleId, vector);
        }

        private void AddPlayerAttackComponent(string playerGoogleId, float angle)
        {
            StaticInputMessagesSorter.AttackMessages.TryAdd(playerGoogleId, angle);
        }
    }
}