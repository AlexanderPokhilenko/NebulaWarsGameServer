using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage;
using Server.GameEngine.MatchLifecycle;
using SharedSimulationCode;
using SharedSimulationCode.Systems;
using UnityEditor.Experimental.GraphView;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сущности ввода игрока в контекстах.
    /// </summary>
    public class InputEntitiesCreator
    {
        private readonly MatchesStorage matchesStorage;
        private readonly ConcurrentStack<InputMessagesPack> inputMessages = new ConcurrentStack<InputMessagesPack>();

        public InputEntitiesCreator(MatchesStorage matchesStorage)
        {
            this.matchesStorage = matchesStorage;
        }

        public void AddInputMessage(InputMessagesPack message)
        {
            inputMessages.Push(message);
        }

        public void Create()
        {
            while (inputMessages.TryPop(out InputMessagesPack inputMessagesPack))
            {
                InputReceiver inputReceiver = null;
                ushort playerTmpId = inputMessagesPack.TemporaryId;
                if (matchesStorage.TryGetMatchInputReceiver(inputMessagesPack.MatchId, ref inputReceiver))
                {
                    foreach (KeyValuePair<int, InputMessageModel> keyValuePair in inputMessagesPack.History)
                    {
                        int inputMessageId = keyValuePair.Key;
                        InputMessageModel inputModel = keyValuePair.Value;

                        if (!inputReceiver.NeedHandle(playerTmpId, inputMessageId, inputModel.TickNumber))
                        {
                            //Сообщение старое или уже обработано
                            continue;
                        }
                        inputReceiver.AddMovement(playerTmpId, inputModel.GetVector2(), inputModel.TickNumber);
                        inputReceiver.AddAttack(playerTmpId, inputModel.Angle, inputModel.TickNumber);
                        if (inputModel.UseAbility)
                        {
                            inputReceiver.AddAbility(playerTmpId, inputModel.TickNumber);
                        }
                    }
                    
                }
            }
            
            inputMessages.Clear();
        }
    }
}