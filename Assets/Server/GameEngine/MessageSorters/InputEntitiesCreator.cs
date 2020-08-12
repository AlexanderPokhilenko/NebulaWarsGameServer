using System.Collections.Concurrent;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using Server.GameEngine.MatchLifecycle;

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
            while (inputMessages.TryPop(out var inputMessage))
            {
                if (matchStorage.TryGetMatch(inputMessage.MatchId, out var match))
                {
                    match.AddMovement(inputMessage.TemporaryId, inputMessage.GetVector2());
                    match.AddAttack(inputMessage.TemporaryId, inputMessage.Angle);
                    if (inputMessage.UseAbility)
                    {
                        match.AddAbility(inputMessage.TemporaryId);
                    }
                }
            }
            
            inputMessages.Clear();
        }
    }
}