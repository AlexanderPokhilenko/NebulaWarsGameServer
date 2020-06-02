using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Code.Common;

namespace Server.Http
{
    /// <summary>
    /// Уведомляет матчмейкер о смертях игроков и окончании матчей.
    /// Если сообщение не отправилось, то попробует заново.
    /// </summary>
    public class MatchmakerNotifier
    {
        private readonly ILog log = LogManager.CreateLogger(typeof(MatchmakerNotifier));
        
        private readonly ConcurrentQueue<int> finishedMatches = new ConcurrentQueue<int>();
        private readonly ConcurrentQueue<PlayerDeathData> killedPlayers = new ConcurrentQueue<PlayerDeathData>();
        
        private readonly HttpMatchFinishNotifierService httpMatchFinishNotifierService;
        private readonly  PlayerDeathMessageValidator playerDeathMessageValidator;
        private readonly MatchmakerPlayerDeathNotifierService matchmakerPlayerDeathNotifierService;
        
        public MatchmakerNotifier()
        {
            HttpWrapper httpWrapper = new HttpWrapper();
            httpMatchFinishNotifierService = new HttpMatchFinishNotifierService(httpWrapper);
            playerDeathMessageValidator = new PlayerDeathMessageValidator();
            matchmakerPlayerDeathNotifierService = new MatchmakerPlayerDeathNotifierService(httpWrapper);
        }
        
        public void MarkMatchAsFinished(int matchId)
        {
            finishedMatches.Enqueue(matchId);
        }

        public void MarkPlayerAsDeath(PlayerDeathData playerDeathData)
        {
            killedPlayers.Enqueue(playerDeathData);
        }
        
        public Thread StartThread()
        {
            Thread thread = new Thread(() => StartEndlessLoop().Wait())
            {
                IsBackground = true
            };
            thread.Start();
            return thread;
        }
        
        private async Task StartEndlessLoop()
        {
            while (true)
            {
                await SendOut();
                await Task.Delay(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private async Task SendOut()
        {
            while (!finishedMatches.IsEmpty)
            {
                finishedMatches.TryDequeue(out int matchId);
                await SendMatchFinishMessage(matchId);
            }
            
            while (!killedPlayers.IsEmpty)
            {
                killedPlayers.TryDequeue(out var playerDeathData);
                playerDeathMessageValidator.Validate(playerDeathData);
                await SendPlayerDeathMessage(playerDeathData);
            }
        }

        private async Task SendMatchFinishMessage(int matchId)
        {
            bool success = await httpMatchFinishNotifierService.Notify(matchId);
            //Если http не справился, то повторить попытку
            if (!success)
            {
                finishedMatches.Enqueue(matchId);
            }
        }
        
        private async Task SendPlayerDeathMessage(PlayerDeathData playerDeathData)
        {
            bool success = await matchmakerPlayerDeathNotifierService.Notify(playerDeathData);
            //Если http не справился, то повторить попытку
            if (!success)
            {
                killedPlayers.Enqueue(playerDeathData);
            }
        }
    }
}