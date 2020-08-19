using System;
using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;
using Server.GameEngine.MatchLifecycle;
using Server.Http;

namespace Server.GameEngine.Experimental.Systems
{
    /// <summary>
    /// Если игрок вышел преждевременно (через кнопку), то эта система
    /// 1) сообщит матчмейкеру о его месте в бою (как будто он умер в этот кадр)
    /// 2) удалит ip игрока из этого матча, чтобы он мог начать новый матч на этом сервере
    /// 3) передаст бренное тело игрока под управления AI 
    /// </summary>
    public class PlayerExitSystem:ReactiveSystem<InputEntity>
    {
        private readonly int matchId;
        private readonly GameContext gameContext;
        private readonly MatchRemover matchRemover;
        private readonly IGroup<InputEntity> playerExitGroup;
        private readonly IGroup<GameEntity> alivePlayersGroup;
        private readonly PlayerDeathHandler playerDeathHandler;
        private readonly IGroup<GameEntity> alivePlayerAndBotsGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(PlayerExitSystem));

        public PlayerExitSystem(Contexts contexts, int matchId, PlayerDeathHandler playerDeathHandler,
            MatchRemover matchRemover)
            :base(contexts.input)
        {
            gameContext = contexts.game;
            alivePlayerAndBotsGroup = gameContext.GetGroup(GameMatcher.Player);
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;
            this.matchRemover = matchRemover;
            alivePlayersGroup = contexts.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.Player).
                    NoneOf(GameMatcher.KilledBy, GameMatcher.Bot));
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
            log.Warn("Вызов реактивной системы для преждевременном удалении игрока из матча");
            foreach (var inputEntity in entities)
            {
                var temporaryId = inputEntity.player.id;
                var playerEntity = gameContext.GetEntityWithPlayer(temporaryId);
                if (playerEntity == null || !playerEntity.hasAccount)
                {
                    log.Warn($"Попытка удалить несуществующего (удалённого?) игрока из матча {nameof(temporaryId)} {temporaryId}");
                }
                var accountId = playerEntity.account.id;
                log.Warn($"преждевременное удаление игрока из матча {nameof(temporaryId)} {temporaryId} {nameof(accountId)} {accountId}");
                TurnIntoBot(temporaryId);
                SendDeathMessage(accountId, temporaryId);
            }
            
            if (alivePlayersGroup.count == 0)
            {
                //живых игроков не осталось. остались только боты
                //чем закончится драка ботов неинтересно матч можно завершать
                log.Info("Живые игроки в матче кончились.");
                matchRemover.MarkMatchAsFinished(matchId);
            }
        }

        private void TurnIntoBot(ushort playerId)
        {
            var playerEntity = gameContext.GetEntityWithPlayer(playerId);
            if (playerEntity != null && !playerEntity.isBot)
            {
                throw new NotImplementedException();
                // Match.MakeBot(playerEntity);
            }
        }

        private void SendDeathMessage(int accountId, ushort temporaryId)
        {
            int placeInBattle = alivePlayerAndBotsGroup.count;
            PlayerDeathData playerDeathData = new PlayerDeathData
            {
                PlayerAccountId = accountId,
                PlaceInBattle = placeInBattle,
                MatchId = matchId 
            };

            playerDeathHandler.PlayerDeath(playerDeathData, temporaryId, false);
        }
    }
}