using System.Collections.Generic;
using System.Linq;
using Entitas;

namespace Server.GameEngine.Systems
{
    public class FinishBattleSystem:ReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Battle battle;

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
            int playersNumber = playersGroup.AsEnumerable().Count();
            switch (playersNumber)
            {
                case 0:
                    //все сдохли
                    battle.StopTicks();
                    break;
                case 1 :
                    //есть победитель
                    battle.StopTicks();
                    break;
            }
        }
    }
}