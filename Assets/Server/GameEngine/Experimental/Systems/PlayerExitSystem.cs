using System.Collections.Generic;
using Code.Common;
using Entitas;
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
        private static readonly ILog Log = LogManager.CreateLogger(typeof(PlayerExitSystem));
    
        private readonly IGroup<GameEntity> alivePlayerAndBotsGroup;
        private readonly IGroup<InputEntity> playerExitGroup;
        private readonly int matchId;
        private readonly PlayerDeathHandler playerDeathHandler;
        private readonly GameContext gameContext;
        
        public PlayerExitSystem(Contexts contexts, int matchId, PlayerDeathHandler playerDeathHandler)
            :base(contexts.input)
        {
            gameContext = contexts.game;
            alivePlayerAndBotsGroup = gameContext.GetGroup(GameMatcher.Player);
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;
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
                TurnIntoBot(playerId);
                SendDeathMessage(playerId);
            }
        }

        private void TurnIntoBot(ushort playerId)
        {
            var playerEntity = gameContext.GetEntityWithPlayer(playerId);
            if(playerEntity != null && !playerEntity.isBot) Match.MakeBot(playerEntity);
        }

        private void SendDeathMessage(ushort playerId)
        {
            int placeInBattle = alivePlayerAndBotsGroup.count;
            PlayerDeathData playerDeathData = new PlayerDeathData
            {
                PlayerTemporaryId = playerId,
                PlaceInBattle = placeInBattle,
                MatchId = matchId 
            };
                
            playerDeathHandler.PlayerDeath(playerDeathData, false);
        }
    }
}