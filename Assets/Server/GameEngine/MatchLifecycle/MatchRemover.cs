using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Common;
using Server.GameEngine.Rudp;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using SharedSimulationCode;

namespace Server.GameEngine.MatchLifecycle
{
    /// <summary>
    /// Правильно очищает данные и убивает матч, номер которого положили в очередь.
    /// </summary>
    public class MatchRemover
    {
        private readonly MatchStorage matchStorage;
        private readonly MessageIdFactory messageIdFactory;
        /// <summary>
        /// Очередь на удаление матча.
        /// </summary>
        private readonly ConcurrentQueue<int> matchesToRemove;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ByteArrayRudpStorage byteArrayRudpStorage;
        private readonly MessagesPackIdFactory messagesPackIdFactory;
        private readonly PlayersMatchFinishNotifier playersMatchFinishNotifier;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchRemover));

        public MatchRemover(MatchStorage matchStorage, ByteArrayRudpStorage byteArrayRudpStorage, 
            UdpSendUtils udpSendUtils, MatchmakerNotifier matchmakerNotifier, IpAddressesStorage ipAddressesStorage,
            MessageIdFactory messageIdFactory, MessagesPackIdFactory messagesPackIdFactory)
        {
            this.messagesPackIdFactory = messagesPackIdFactory;
            this.matchStorage = matchStorage;
            this.byteArrayRudpStorage = byteArrayRudpStorage;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
            this.messageIdFactory = messageIdFactory;
            matchesToRemove = new ConcurrentQueue<int>();
            playersMatchFinishNotifier = new PlayersMatchFinishNotifier(udpSendUtils, ipAddressesStorage);
        }
        
        public void MarkMatchAsFinished(int matchId)
        {
            if (!matchesToRemove.Contains(matchId))
            {
                log.Info("Добавление матча для в очередь для удаления.");
                matchesToRemove.Enqueue(matchId);
            }
            else
            {
                log.Info("Попытка добавить матч в очередь для удаления");
            }
        }
        
        //TODO изолировать этот метод. он сейчас доступен из систем
        public void DeleteFinishedMatches()
        {
            while (matchesToRemove.Count != 0)
            {
                if (matchesToRemove.TryDequeue(out int matchId))
                {
                    log.Info("Удаление боя "+matchId);
                    DeleteMatch(matchId);    
                }
            }
        }
        
        /// <summary>
        /// Удаляет матч из памяти. Уведомляет игроков и матчмейкер о конце матча.
        /// </summary>
        private void DeleteMatch(int matchId)
        {
            MatchSimulation match = matchStorage.DequeueMatch(matchId);
            if (match == null)
            {
                log.Error($"Матч уже был удалён. {nameof(matchId)} {matchId}");
                return;
            }
            
            playersMatchFinishNotifier.Notify(match);
            match.TearDown();
            matchmakerNotifier.MarkMatchAsFinished(matchId);
            
            Task.Run(async () =>
            {
                //todo вынести это в новый класс
                //задержка нужна для того, чтобы последние udp сообщения дошли до игроков
                await Task.Delay(10_000);
                log.Info($"Удаление rudp сообщений для матча {nameof(matchId)} {matchId}.");
                byteArrayRudpStorage.RemoveMatchMessages(matchId);
                List<ushort> playersIds = ipAddressesStorage.GetActivePlayersIds(matchId);
                if (playersIds != null)
                {
                    foreach (ushort playerId in playersIds)
                    {
                        messageIdFactory.RemovePlayer(matchId, playerId);
                        messagesPackIdFactory.RemovePlayer(matchId, playerId);
                    }
                }
            });
        }
    }
}