using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Code.Common;
using Libraries.Logger;

namespace Server.Http
{
    /// <summary>
    /// Отправляет матчмейкеру сообщения о исключении игрока из мачта и окончании матча.
    /// Если сообщение не отправилось, то попробует заново.
    /// </summary>
    public class MatchmakerNotifier
    {
        private const int PeriodMs = 1000;
        private readonly PlayerDeathMessageValidator playerDeathMessageValidator;
        private readonly HttpMatchFinishNotifierService httpMatchFinishNotifierService;
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchmakerNotifier));
        private readonly MatchmakerPlayerDeathNotifierService matchmakerPlayerDeathNotifierService;
        
        //matchId + null
        private readonly ConcurrentDictionary<int, object> finishedMatchIdCollection = 
            new ConcurrentDictionary<int, object>();
        //PlayerDeathData + null
        private readonly ConcurrentDictionary<PlayerDeathData, object> killedPlayerCollection = 
            new ConcurrentDictionary<PlayerDeathData, object>();
        
        public MatchmakerNotifier()
        {
            HttpWrapper httpWrapper = new HttpWrapper();
            playerDeathMessageValidator = new PlayerDeathMessageValidator();
            httpMatchFinishNotifierService = new HttpMatchFinishNotifierService(httpWrapper);
            matchmakerPlayerDeathNotifierService = new MatchmakerPlayerDeathNotifierService(httpWrapper);
        }
        
        /// <summary>
        /// Вызывается, когда матч окончен
        /// </summary>
        /// <param name="matchId"></param>
        public void MarkMatchAsFinished(int matchId)
        {
            if (!finishedMatchIdCollection.TryAdd(matchId, null))
            {
                log.Error($"Попытка повторно добавить матч с {nameof(matchId)} {matchId} в коллекцию матчей" +
                          " о завершении которых нужно сообщить матчмейкеру.");
            }
        }

        /// <summary>
        /// Вызывается, когда игрок умер или покинул матч или был исключен из матча.
        /// </summary>
        /// <param name="playerDeathData"></param>
        public void MarkPlayerAsExcluded(PlayerDeathData playerDeathData)
        {
            if (!killedPlayerCollection.TryAdd(playerDeathData, null))
            {
                log.Error($"Попытка повторно добавить игрока с {nameof(playerDeathData.PlayerAccountId)} " +
                          $"{playerDeathData.PlayerAccountId} в коллекцию игроков которые были исключены из матча.");
            }
        }
        
        public CancellationTokenSource StartThread()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Thread thread = new Thread(async () => await EndlessLoop(token))
            {
                IsBackground = true
            };
            thread.Start();
            return cts;
        }
        
        private async Task EndlessLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (int matchId in finishedMatchIdCollection.Select(pair=>pair.Key))
                {
                    if (await httpMatchFinishNotifierService.TryNotify(matchId))
                    {
                        finishedMatchIdCollection.TryRemove(matchId, out _);
                    }
                }
            
                foreach (PlayerDeathData playerDeathData in killedPlayerCollection.Select(pair=>pair.Key))
                {
                    playerDeathMessageValidator.Validate(playerDeathData);
                    if (await matchmakerPlayerDeathNotifierService.TryNotify(playerDeathData))
                    {
                        killedPlayerCollection.TryRemove(playerDeathData, out _);
                    }
                }
                
                await Task.Delay(PeriodMs, token);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}