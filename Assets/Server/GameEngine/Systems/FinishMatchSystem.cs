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
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Match match;

        public FinishMatchSystem(Contexts contexts, Match match) : base(contexts.game)
        {
            playersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.KilledBy));
            this.match = match;
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
            
            Log.Warn($" {nameof(match.matchData.MatchId)} {match.matchData.MatchId} " +
                     $"Погибло игровых сущностей: {entities.Count}. " +
                     $"Текущее кол-во игровых сущностей: {numberOfAlivePlayers}");
            foreach (var gameEntity in entities)
            {
                if (gameEntity.isBot)
                {
                    Log.Warn($"{nameof(match.matchData.MatchId)} {match.matchData.MatchId} " +
                             $"Погиб бот {gameEntity.viewType.id}");
                }
                else if(gameEntity.hasPlayer)
                {
                    Log.Warn($"{nameof(match.matchData.MatchId)} {match.matchData.MatchId}" +
                             $"  Погиб игрок {gameEntity.player.id}");
                }
                else
                {
                    Log.Warn("Было убито хер знает что");
                }
            }

            switch (numberOfAlivePlayers)
            {
                case 0:
                    //последние игроки сдохли в одном кадре
                    match.FinishGame();
                    break;
                case 1 :
                    //есть победитель
                    match.FinishGame();
                    break;
            }
        }
    }
}