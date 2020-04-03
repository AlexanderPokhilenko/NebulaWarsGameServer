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
            Log.Warn(nameof(FinishMatchSystem)+" ctor");
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
            Log.Warn($"{nameof(FinishMatchSystem)}");
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
                    foreach (var gameEntity in entities)
                    {
                        Console.WriteLine($"{nameof(gameEntity.player.id)} {gameEntity.player.id}");
                    }
                    Log.Warn("Список погибших за этот кард окончен.");
                    break;
            }
        }
    }
}