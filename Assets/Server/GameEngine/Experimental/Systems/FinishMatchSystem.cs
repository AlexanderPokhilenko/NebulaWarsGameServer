using System.Collections.Generic;
using Entitas;
using log4net;

namespace Server.GameEngine.Systems
{
    /// <summary>
    /// Вызывает удаление матча, когда остаётся 0 или 1 игрок
    /// </summary>
    public class FinishMatchSystem:ReactiveSystem<GameEntity>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FinishMatchSystem));
        
        private readonly MatchRemover matchRemover;
        private readonly int matchId;
        private readonly IGroup<GameEntity> playersGroup;
        
        public FinishMatchSystem(Contexts contexts, MatchRemover matchRemover, int matchId) 
            : base(contexts.game)
        {
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.KilledBy));
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
            int numberOfAlivePlayers = playersGroup.count;
            LogKilledEntities(entities, numberOfAlivePlayers);

            switch (numberOfAlivePlayers)
            {
                case 0:
                    //последние игроки сдохли в одном кадре
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
                case 1 :
                    //есть победитель
                    matchRemover.MarkMatchAsFinished(matchId);
                    break;
            }
        }

        private void LogKilledEntities(List<GameEntity> entities, int numberOfAlivePlayers)
        {
            Log.Warn($" {nameof(matchId)} {matchId} " +
                     $"Погибло игровых сущностей: {entities.Count}. " +
                     $"Текущее кол-во игровых сущностей: {numberOfAlivePlayers}");
            foreach (var gameEntity in entities)
            {
                if (gameEntity.isBot)
                {
                    Log.Warn($"{nameof(matchId)} {matchId} Погиб бот {gameEntity.viewType.id}");
                }
                else if(gameEntity.hasPlayer)
                {
                    Log.Warn($"{nameof(matchId)} {matchId} Погиб игрок {gameEntity.player.id}");
                }
                else
                {
                    Log.Warn("Было убито хер знает что");
                }
            }
        }
    }
}