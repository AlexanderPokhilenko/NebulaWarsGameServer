using System.Collections.Concurrent;
using log4net;
using Server.Http;
using Server.Udp.Storage;

namespace Server.GameEngine
{
    /// <summary>
    /// Правильно очищает данные и убивает матч, номер которого положили в очередь.
    /// </summary>
    public class MatchRemover
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchRemover));
        
        /// <summary>
        /// Очередь на удаление матча.
        /// </summary>
        private readonly ConcurrentQueue<int> matchesToRemove;
        private readonly MatchStorage matchStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;

        public MatchRemover(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage)
        {
            this.matchStorage = matchStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
            matchesToRemove = new ConcurrentQueue<int>();
        }
        
        public void MarkMatchAsFinished(int matchNumber)
        {
            matchesToRemove.Enqueue(matchNumber);
        }
        
        public void DeleteFinishedMatches()
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
            Match match = matchStorage.RemoveMatch(matchId);
            match.NotifyPlayersAboutMatchFinish();
            match.TearDown();
            MatchDeletingNotifier.SendMatchDeletingMessage(matchId);
            byteArrayRudpStorage.RemoveMatchMessages(matchId);
        }
    }
}