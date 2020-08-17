using Entitas;
using Entitas.Unity;

namespace SharedSimulationCode
{
    public class DestroyEntitySystem:ICleanupSystem
    {
        private readonly PhysicsDestroyer physicsDestroyer;
        private readonly IGroup<GameEntity> needDestroyGroup;

        public DestroyEntitySystem(Contexts contexts, PhysicsDestroyer physicsDestroyer)
        {
            this.physicsDestroyer = physicsDestroyer;
            needDestroyGroup = contexts.game.GetGroup(GameMatcher.Destroyed);
        }

        public void Cleanup()
        {
            var needDestroy = needDestroyGroup.GetEntities();
            for (int i = 0; i < needDestroy.Length; i++)
            {
                var entity = needDestroy[i];
                if (entity.hasTransform)
                {
                    var go = entity.transform.value.gameObject;
                    go.Unlink();
                    physicsDestroyer.Destroy(go);
                }
                
                entity.Destroy();
            }
        }
    }
}