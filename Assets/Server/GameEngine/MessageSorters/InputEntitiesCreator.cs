﻿using System.Collections.Concurrent;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.PlayerToServer;
using Plugins.submodules.SharedCode.Systems;
using Server.GameEngine.MatchLifecycle;

namespace Server.GameEngine.MessageSorters
{
    /// <summary>
    /// Создаёт сущности ввода игрока в контекстах.
    /// </summary>
    public class InputEntitiesCreator
    {
        private readonly MatchesStorage matchesStorage;
        private readonly ConcurrentStack<InputMessagesPack> inputMessages = 
            new ConcurrentStack<InputMessagesPack>();

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
                    foreach (var pair in inputMessagesPack.History)
                    {
                        uint inputMessageId = pair.Key;
                        InputMessageModel inputMessageModel = pair.Value;

                        inputReceiver.AddMessage(playerTmpId, inputMessageId, inputMessageModel);
                     }
                }
            }
            
            inputMessages.Clear();
        }
    }
}