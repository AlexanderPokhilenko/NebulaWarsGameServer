using System.Collections.Concurrent;
using System.Threading.Tasks;
using log4net;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Правильно очищает данные и убивает матч, номер которого положили в очередь.
    /// </summary>
    public class MatchRemover
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MatchRemover));
        
        /// <summary>
        /// Очередь на удаление матча.
        /// </summary>
        private readonly ConcurrentQueue<int> matchesToRemove;
        private readonly MatchStorage matchStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;
        private readonly MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier;
        private readonly PlayersMatchFinishNotifier playersMatchFinishNotifier;

        public MatchRemover(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, 
            UdpSendUtils udpSendUtils, MatchmakerMatchStatusNotifier matchmakerMatchStatusNotifier)
        {
            this.matchStorage = matchStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
            this.matchmakerMatchStatusNotifier = matchmakerMatchStatusNotifier;
            matchesToRemove = new ConcurrentQueue<int>();
            playersMatchFinishNotifier = new PlayersMatchFinishNotifier(udpSendUtils);
        }
        
        public void MarkMatchAsFinished(int matchId)
        {
            matchesToRemove.Enqueue(matchId);
        }
        
        //TODO изолировать этот метод. он сейчас доступен из систем
        public void DeleteFinishedMatches()
        {
            while (matchesToRemove.Count != 0)
            {
                if (matchesToRemove.TryDequeue(out int matchId))
                {
                    log.Warn("Удаление боя "+matchId);
                    DeleteMatch(matchId);    
                }
            }
        }
        
        /// <summary>
        /// Удаляет матч из памяти. Уведомляет игроков и матчмейкер о конце матча.
        /// </summary>
        private void DeleteMatch(int matchId)
        {
            log.Warn($"{nameof(DeleteMatch)} {nameof(matchId)} {matchId}");
            Match match = matchStorage.DequeueMatch(matchId);
            playersMatchFinishNotifier.Notify(match);
            match.TearDown();
            matchmakerMatchStatusNotifier.MarkMatchAsFinished(matchId);
            
            log.Info($"Перед удалением сообщений для матча {nameof(matchId)} {matchId}.");
            Task.Run(async () =>
            {
                //задержка нужна для того, чтобы последние udp сообщения дошли до игроков
                await Task.Delay(1000);
                byteArrayRudpStorage.RemoveMatchMessages(matchId);
            });
        }
    }
}