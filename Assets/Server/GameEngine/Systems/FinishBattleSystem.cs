using System.Collections.Generic;
using System.Linq;
using Entitas;
using log4net;


namespace Server.GameEngine.Systems
{
    public class FinishBattleSystem:ReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Match match;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatchStorage));

        public FinishBattleSystem(Contexts contexts, Match match) : base(contexts.game)
        {
            playersGroup = contexts.game.GetGroup(GameMatcher.Player);
            this.match = match;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Player.Removed());
        }

        protected override bool Filter(GameEntity entity)
        {
            return !entity.hasPlayer;
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
                    break;
            }
        }
    }
}