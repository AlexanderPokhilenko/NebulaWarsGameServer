using System.Collections.Generic;
using System.Linq;
using Entitas;
using log4net;


namespace Server.GameEngine.Systems
{
    public class FinishBattleSystem:ReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Battle battle;
        private static readonly ILog Log = LogManager.GetLogger(typeof(BattlesStorage));

        public FinishBattleSystem(Contexts contexts, Battle battle) : base(contexts.game)
        {
            playersGroup = contexts.game.GetGroup(GameMatcher.Player);
            this.battle = battle;
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
                    battle.FinishGame();
                    break;
                case 1 :
                    //есть победитель
                    battle.FinishGame();
                    break;
                default:
                    Log.Warn("Минус игрок. Текущее кол-во: "+numberOfPlayers);
                    break;
            }
        }
    }
}