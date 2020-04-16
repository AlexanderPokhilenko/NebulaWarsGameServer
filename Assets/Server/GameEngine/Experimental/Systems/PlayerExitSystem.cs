using System;
using System.Collections.Generic;
using Entitas;
using log4net;
using Server.Http;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Если игрок вышел преждевременно (через кнопку), то эта система
    /// 1) сообщит матчмейкеру о его месте в бою (как будто он умер в этот кадр)
    /// 2) удалит ip игрока из этого матча, чтобы он мог начать новый матч на этом сервере
    /// 3) передаст бренное тело игрока под управления AI 
    /// </summary>
    public class PlayerExitSystem:ReactiveSystem<InputEntity>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitSystem));
    
        private readonly IGroup<GameEntity> alivePlayerAndBotsGroup;
        private readonly IGroup<InputEntity> playerExitGroup;
        private readonly int matchId;
        private readonly Match match;
        
        public PlayerExitSystem(Contexts contexts, int matchId, Match match)
            :base(contexts.input)
        {
            var gameContext = contexts.game;
            alivePlayerAndBotsGroup = gameContext.GetGroup(GameMatcher.Player);
            playerExitGroup = contexts.input.GetGroup(InputMatcher.LeftTheGame);
            this.matchId = matchId;
            this.match = match;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.LeftTheGame.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isLeftTheGame && entity.hasPlayer;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            Log.Warn("Вызов реактивной системы для преждевременном удалении игрока из матча");
            foreach (var inputEntity in entities)
            {
                var playerId = inputEntity.player.id;
                Log.Warn($"преждевременное удаление игрока из матча {nameof(playerId)} {playerId}");
                //TODO Пометить сущность игрока для передачи управления в AI системы.
                RemoveFromActivePlayers(playerId);
                SendPlayerDeathMessageToMatchmaker(playerId);
            }
        }

        private void RemoveFromActivePlayers(int playerId)
        {
            bool success = match.TryRemoveIpEndPoint(playerId);
            if (!success)
            {
                throw new Exception("Получив это сообщение, я буду пребывать в крайне скудном расположении духа.");
            }
        }
        
        private void SendPlayerDeathMessageToMatchmaker(int playerId)
        {
            int placeInBattle = alivePlayerAndBotsGroup.count;
            PlayerDeathData playerDeathData = new PlayerDeathData
            {
                PlayerId = playerId,
                PlaceInBattle = placeInBattle,
                MatchId = matchId 
            };
            PlayerDeathNotifier.KilledPlayers.Enqueue(playerDeathData);
        }
    }
}