using System.Collections.Concurrent;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using Server.GameEngine.MatchLifecycle;
using SharedSimulationCode;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сущности ввода игрока в контекстах.
    /// </summary>
    public class InputEntitiesCreator
    {
        private readonly MatchStorage matchStorage;
        private readonly ConcurrentStack<PlayerInputMessage> inputMessages = new ConcurrentStack<PlayerInputMessage>();

        public InputEntitiesCreator(MatchStorage matchStorage)
        {
            this.matchStorage = matchStorage;
        }

        public void AddInputMessage(PlayerInputMessage message)
        {
            inputMessages.Push(message);
        }

        public void Create()
        {
            while (inputMessages.TryPop(out PlayerInputMessage inputMessage))
            {
                InputReceiver inputReceiver = null;
                if (matchStorage.TryGetMatchInputReceiver(inputMessage.MatchId, ref inputReceiver))
                {
                    inputReceiver.AddMovement(inputMessage.TemporaryId, inputMessage.GetVector2());
                    inputReceiver.AddAttack(inputMessage.TemporaryId, inputMessage.Angle);
                    if (inputMessage.UseAbility)
                    {
                        inputReceiver.AddAbility(inputMessage.TemporaryId);
                    }
                }
            }
            
            inputMessages.Clear();
        }
    }
}