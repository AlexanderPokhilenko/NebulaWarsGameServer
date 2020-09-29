using Entitas;
using Plugins.submodules.SharedCode.Logger;

namespace Server.GameEngine
{
    public class LowHpCheckSystem : IExecuteSystem
    {
        private readonly IGroup<ServerGameEntity> withHpGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(LowHpCheckSystem));

        public LowHpCheckSystem(Contexts contexts)
        {
            withHpGroup = contexts.serverGame.GetGroup(ServerGameMatcher.HealthPoints);
        }

        public void Execute()
        {
            foreach (var withHp in withHpGroup)
            {
                if (withHp.healthPoints.value <= 0)
                {
                    log.Debug("Слишком мало очков прочности. Удаление корабля");
                    withHp.isDestroyed = true;
                }
            }
        }
    }
}