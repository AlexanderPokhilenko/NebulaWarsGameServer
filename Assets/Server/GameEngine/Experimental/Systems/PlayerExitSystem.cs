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
    //TODO тут нужно вызывать Cleanup ?
    public class PlayerExitSystem:ReactiveSystem<InputEntity>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerExitSystem));
    
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
            // playerExitGroup = contexts.input.GetGroup(InputMatcher.PlayerExit);
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;

        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            throw new NotImplementedException();
            // return context.CreateCollector(InputMatcher.PlayerExit.Added());
        }

        protected override bool Filter(InputEntity entity)
        {
            throw new NotImplementedException();
            // return entity.hasPlayerExit;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            Log.Warn("Вызов реактивной системы для преждевременном удалении игрока из матча");
            foreach (var inputEntity in entities)
            {
                throw new NotImplementedException();
                // int playerId = inputEntity.playerExit.PlayerId;
                // Log.Warn($"преждевременное удаление игрока из матча {nameof(playerId)} {playerId}");
                // //TODO Пометить сущность игрока для передачи управления в AI системы.
                //
                // var playerEntity = gameContext.GetEntityWithPlayer(playerId);
                // if(playerEntity != null) Match.MakeBot(playerEntity);
                //
                //
                // int placeInBattle = alivePlayerAndBotsGroup.count;
                // PlayerDeathData playerDeathData = new PlayerDeathData
                // {
                //     PlayerId = playerId,
                //     PlaceInBattle = placeInBattle,
                //     MatchId = matchId 
                // };
                //
                // playerDeathHandler.PlayerDeath(playerDeathData, true);
            }
        }
    }
}