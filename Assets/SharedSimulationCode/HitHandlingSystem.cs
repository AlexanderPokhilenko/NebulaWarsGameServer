using System;
using System.Runtime.InteropServices;
using Code.Common;
using Entitas;

namespace SharedSimulationCode
{
    /// <summary>
    /// Обрабатывает сущность столкновения.
    /// Отнимает здоровье у корабля и уничтожает снаряд.
    /// </summary>
    public class HitHandlingSystem : IExecuteSystem, ICleanupSystem
    {
        private readonly IGroup<GameEntity> hitGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(HitHandlingSystem));

        public HitHandlingSystem(Contexts contexts)
        {
            hitGroup = contexts.game.GetGroup(GameMatcher.Hit);
        }

        public void Execute()
        {
            foreach (var entity in hitGroup)
            {
                GameEntity warshipEntity = entity.hit.warshipEntity;
                GameEntity projectileEntity = entity.hit.projectileEntity;

                var parentId = projectileEntity.parentWarship.entity.id.value;
                if (parentId == warshipEntity.id.value)
                {
                    throw new Exception("Попадание по родителю не должно считаться");
                }
                
                //Отнять хп корабля
                float actualHealthPoints = warshipEntity.healthPoints.value - projectileEntity.damage.value;
                warshipEntity.ReplaceHealthPoints(actualHealthPoints);
                
                //Уничтожить снаряд
                projectileEntity.isDestroyed = true;
            }
        }

        public void Cleanup()
        {
            var entities = hitGroup.GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i].Destroy();
            }
        }
    }
}