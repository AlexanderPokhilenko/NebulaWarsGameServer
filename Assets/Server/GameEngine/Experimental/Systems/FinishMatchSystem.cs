using System.Collections.Generic;
using Code.Common;
using Entitas;
using Server.GameEngine.MatchLifecycle;

namespace Server.GameEngine.Experimental.Systems
{
    /// <summary>
    /// Вызывает удаление матча, когда остаётся мало игроков
    /// </summary>
    public class FinishMatchSystem:ReactiveSystem<GameEntity>
    {
        private readonly int matchId;
        private readonly MatchRemover matchRemover;
        private readonly IGroup<GameEntity> alivePlayersGroup;
        private readonly IGroup<GameEntity> alivePlayersAndBotsGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(FinishMatchSystem));
        
        public FinishMatchSystem(Contexts contexts, MatchRemover matchRemover, int matchId) 
            : base(contexts.game)
        {
            alivePlayersGroup = contexts.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.Player).
                    NoneOf(GameMatcher.KilledBy, GameMatcher.Bot));
            
            alivePlayersAndBotsGroup = contexts.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.Player).
                    NoneOf(GameMatcher.KilledBy));
            
            this.matchRemover = matchRemover;
            this.matchId = matchId;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.KilledBy.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasKilledBy && entity.hasPlayer;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            LogKilledEntities(entities);

            switch (alivePlayersAndBotsGroup.count)
            {
                case 0:
                    //последние игроки погибли в одном кадре
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
                case 1 :
                    //есть победитель
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
            }

            if (alivePlayersGroup.count == 0)
            {
                //живых игроков не осталось. остались только боты
                //чем закончится драка ботов неинтересно матч можно завершать
                log.Info("Живые игроки в матче кончились.");
                matchRemover.MarkMatchAsFinished(matchId);
            }
        }

        private void LogKilledEntities(List<GameEntity> entities)
        {
            log.Info($" {nameof(matchId)} {matchId} Погибло игровых сущностей: {entities.Count}. ");
            foreach (var gameEntity in entities)
            {
                if (gameEntity.isBot)
                {
                    log.Info($"{nameof(matchId)} {matchId} Погиб бот {gameEntity.viewType.id}");
                }
                else if(gameEntity.hasPlayer)
                {
                    log.Info($"{nameof(matchId)} {matchId} Погиб игрок {gameEntity.player.id}");
                }
                else
                {
                    log.Error("Был убит неопознанный объект");
                }
            }
        }
    }
}