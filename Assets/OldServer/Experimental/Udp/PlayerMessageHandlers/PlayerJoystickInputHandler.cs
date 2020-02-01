using System.Net;
using AmoebaBattleServer01.Experimental.GameEngine;
using NetworkLibrary.NetworkLibrary.Udp;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using ZeroFormatter;

//TODO заменить vector2

namespace AmoebaBattleServer01.Experimental.PlayerMessageHandlers
{
    public class PlayerJoystickInputHandler
    {
        public void Handle(Message message, IPEndPoint sender)
        {
            PlayerJoystickInputMessage mes =
                ZeroFormatterSerializer.Deserialize<PlayerJoystickInputMessage>(message.SerializedMessage);
            
            AddPlayerInputComponent(mes.PlayerGoogleId, mes.X, mes.Y);
        }

        private void AddPlayerInputComponent(string playerGoogleId, float x, float y)
        {
            StaticInputMessagesSorter.MessagesToBeSynchronized.TryAdd(playerGoogleId, new Vector2(x,y));
        }
    }
}