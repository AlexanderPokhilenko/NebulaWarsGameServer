using Entitas;

namespace Server.GameEngine
{
    /// <summary>
    /// Помечает разрушенные объекты
    /// </summary>
    public class HealthCheckerSystem : IExecuteSystem
    {
        private readonly IGroup<ServerGameEntity> withHealthGroup;

        public HealthCheckerSystem(Contexts contexts)
        {
            withHealthGroup = contexts.serverGame.GetGroup(ServerGameMatcher.HealthPoints);
        }
        
        public void Execute()
        {
            ServerGameEntity[] entities = withHealthGroup.GetEntities();
            for (int index = 0; index < entities.Length; index++)
            {
                var entity = entities[index];
                if (entity.healthPoints.value <= 0)
                {
                    entity.isDestroyed = true;
                }
            }
        }
    }
}