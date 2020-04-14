using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly PlayersMatchFinishNotifier playersMatchFinishNotifier;

        public MatchRemover(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, UdpSendUtils udpSendUtils)
        {
            this.matchStorage = matchStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
            matchesToRemove = new ConcurrentQueue<int>();
            playersMatchFinishNotifier = new PlayersMatchFinishNotifier(udpSendUtils);
        }
        
        public void MarkMatchAsFinished(int matchId)
        {
            matchesToRemove.Enqueue(matchId);
        }
        
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
            MatchmakerMatchFinishNotifier.SendMessage(matchId);
            Task.Run(async () =>
            {
                //задержка нужна для того, чтобы последние udp сообщения дошли до игроков
                await Task.Delay(1000);
                byteArrayRudpStorage.RemoveMatchMessages(matchId);
            });
        }
    }

    /// <summary>
    /// Отправляет игрокам сообщения об окончании матча по udp.
    /// </summary>
    public class PlayersMatchFinishNotifier
    {
        private readonly UdpSendUtils udpSendUtils;
        private readonly ILog log = LogManager.GetLogger(typeof(PlayersMatchFinishNotifier));

        public PlayersMatchFinishNotifier(UdpSendUtils udpSendUtils)
        {
            this.udpSendUtils = udpSendUtils;
        }
        
        public void Notify(Match match)
        {
            List<int> activePlayersIds = match.GetActivePlayersIds();
            if (activePlayersIds.Count == 0)
            {
                log.Info("Список активных игроков пуст. Некого уведомлять о окончании матча.");
            }
            else
            {
                log.Warn(" Старт уведомления игроков про окончание матча");
                foreach (int playerId in activePlayersIds)
                {
                    log.Warn($"Отправка уведомления о завершении боя игроку {nameof(playerId)} {playerId}");
                    udpSendUtils.SendMatchFinishMessage(match.MatchId, playerId);
                }
                log.Warn(" Конец уведомления игроков про окончание матча");
            }
        }
    }
}