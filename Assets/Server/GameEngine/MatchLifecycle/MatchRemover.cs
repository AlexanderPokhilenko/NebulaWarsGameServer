using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine
{
    /// <summary>
    /// Правильно очищает данные и убивает матч, который положили в очередь.
    /// </summary>
    public class MatchRemover
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchRemover));
        
        /// <summary>
        /// Очередь на удаление матча.
        /// </summary>
        private readonly ConcurrentQueue<int> matchesToRemove;

        public MatchRemover()
        {
            matchesToRemove = new ConcurrentQueue<int>();
        }
        
        public void MarkMatchAsFinished(int battleNumber)
        {
            matchesToRemove.Enqueue(battleNumber);
        }
        
        public void DeleteFinishedBattles()
        {
            while (matchesToRemove.Count != 0)
            {
                if (matchesToRemove.TryDequeue(out int matchId))
                {
                    Log.Warn("Удаление боя "+matchId);
                    DeleteMatch(matchId);    
                }
            }
        }
        
        /// <summary>
        /// Удаляет матч из памяти. Уведомляет игроков и матчмейкер о конце матча.
        /// </summary>
        /// <param name="matchId"></param>
        private void DeleteMatch(int matchId)
        {
            Log.Warn($"{nameof(DeleteMatch)} {nameof(matchId)} {matchId}");
            List<int> playersIds = matchStorage.GetActivePlayersIds(matchId);
            PlayersNotifyHelper.Notify(matchId, playersIds);
            
            
            Log.Warn($" Старт уведомления игроков про окончание матча");
            foreach (int playerId in playersIds)
            {
                Log.Warn($"Отправка уведомления о завуршении боя игроку {nameof(playerId)} {playerId}");
                UdpSendUtils.SendBattleFinishMessage(matchId, playerId);
            }
            Log.Warn($" Конец уведомления игроков про окончание матча");
            
            
            
            matchStorage.TearDownMatch(matchId);
            matchStorage.RemoveMatch(matchId);
            MatchDeletingNotifier.SendMatchDeletingMessage(matchId);
        }
        
    }
}