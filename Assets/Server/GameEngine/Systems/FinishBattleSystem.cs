using System;
using System.Collections.Generic;
using Entitas;
using log4net;

namespace Server.GameEngine.Systems
{
    public class FinishBattleSystem:ReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Match match;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorageFacade));

        public FinishBattleSystem(Contexts contexts, Match match) : base(contexts.game)
        {
            playersGroup = contexts.game.GetGroup(GameMatcher.Player);
            this.match = match;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.KilledBy.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasKilledBy;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            int numberOfPlayers = playersGroup.count;
            switch (numberOfPlayers)
            {
                case 0:
                    //все сдохли
                    //такого быть не должно
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