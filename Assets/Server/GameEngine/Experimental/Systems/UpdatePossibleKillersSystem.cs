using System.Collections.Generic;
using Entitas;

namespace Server.GameEngine.Systems
{
    public class UpdatePossibleKillersSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly GameContext gameContext;
        private readonly Dictionary<int, (int playerId, ViewTypeId type)> ownersInfo;

        public UpdatePossibleKillersSystem(Contexts contexts, 
            Dictionary<int, (int playerId, ViewTypeId type)> ownersInfos) 
            : base(contexts.game)
        {
            gameContext = contexts.game;
            ownersInfo = ownersInfos;
        }

        public void Initialize()
        {
            var zone = gameContext.zone.GetZone(gameContext);
            ownersInfo.Add(zone.id.value, (0, zone.viewType.id));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GrandOwner);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGrandOwner;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                var grandOwnerId = e.grandOwner.id;
                if (ownersInfo.ContainsKey(grandOwnerId)) return;
                var grandOwner = gameContext.GetEntityWithId(grandOwnerId);
                if (grandOwner != null && grandOwner.hasViewType)
                {
                    var playerId = 0;
                    var typeId = grandOwner.viewType.id;
                    if (grandOwner.hasAccount) playerId = grandOwner.account.id;
                    ownersInfo.Add(grandOwnerId, (playerId, typeId));
                }
            }
        }
    }
}