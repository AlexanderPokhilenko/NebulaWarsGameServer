using System.Collections.Generic;
using Code.Common;

namespace SharedSimulationCode.Systems
{
    /// <summary>
    /// Нужен для определения того, какие сообщения уже были обработаны, а какие нужно обработать.
    /// </summary>
    public class InputMessagesMetaHistory
    {
        private readonly int maxHashSetLength;
        private readonly Dictionary<int, SortedSet<int>> dictionary;
        private readonly ILog log = LogManager.CreateLogger(typeof(InputMessagesMetaHistory));
        
        public InputMessagesMetaHistory(int maxHashSetLength, List<ushort> playerIds)
        {
            this.maxHashSetLength = maxHashSetLength;
            dictionary = new Dictionary<int, SortedSet<int>>(playerIds.Count);
            foreach (ushort playerId in playerIds)
            {
                dictionary.Add(playerId, new SortedSet<int>());
            }
        }

        public void AddId(ushort playerId, int inputMessageId)
        {
            if (!dictionary.ContainsKey(playerId))
            {
                log.Error("Сообщения от этого игрока не ожидаются");
                return;
            }

            SortedSet<int> sortedSet = dictionary[playerId];
            sortedSet.Add(inputMessageId);
            if (sortedSet.Count > maxHashSetLength)
            {
                sortedSet.Remove(sortedSet.Min);
            }
        }

        public bool NeedHandle(ushort playerId, int inputMessageId)
        {
            if (!dictionary.ContainsKey(playerId))
            {
                log.Error("Сообщения от этого игрока не ожидаются");
                return false;
            }
            
            SortedSet<int> sortedSet = dictionary[playerId];
            
            //Сообщение очень старое
            if (inputMessageId < sortedSet.Min)
            {
                return false;
            }

            //Сообщение уже обработано
            if (sortedSet.Contains(inputMessageId))
            {
                return false;
            }

            return true;
        }
    }
}