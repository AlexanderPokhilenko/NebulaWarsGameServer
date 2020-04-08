using System;
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
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Match match;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FinishMatchSystem));

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
            Log.Info($"{nameof(FinishMatchSystem)}");
            int numberOfPlayers = playersGroup.count;
            switch (numberOfPlayers)
            {
                case 0:
                    //последние игроки сдохли в одном кадре
                    match.FinishGame();
                    break;
                case 1 :
                    //есть победитель
                    match.FinishGame();
                    break;
                default:
                    Log.Warn("Минус игрок. Текущее кол-во: "+numberOfPlayers);
                    Log.Warn("Список погибших за этот кадр.");
                    //TODO почему тут не выводит ботов?
                    foreach (var gameEntity in entities)
                    {
                        if (gameEntity.isBot)
                        {
                            Log.Warn($"Был убит бот {gameEntity.viewType.id}");
                        }
                        else if(gameEntity.hasPlayer)
                        {
                            Log.Warn($"Был убит игрок {gameEntity.player.id}");
                        }
                        else
                        {
                            Log.Warn("Было убито хер знает что");
                        }
                    }
                    Log.Warn("Список погибших за этот кард окончен.");
                    break;
            }
        }
    }
}