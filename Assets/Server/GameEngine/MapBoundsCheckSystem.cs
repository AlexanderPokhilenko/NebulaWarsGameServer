using Entitas;
using Plugins.submodules.SharedCode.Logger;
using UnityEngine;

namespace Server.GameEngine
{
    /// <summary>
    /// Убивает объекты, которые вышли за границы карты
    /// </summary>
    public class MapBoundsCheckSystem : IExecuteSystem
    {
        private readonly IGroup<ServerGameEntity> withTransformGroup;
        private readonly ILog log = LogManager.CreateLogger(typeof(MapBoundsCheckSystem));

        public MapBoundsCheckSystem(Contexts contexts)
        {
            var context = contexts.serverGame;
            withTransformGroup = context.GetGroup(ServerGameMatcher.Transform);
        }

        public void Execute()
        {
            foreach (var entity in withTransformGroup)
            {
                Vector3 position = entity.transform.value.position;
                if (Mathf.Abs(position.x) > 200 || Mathf.Abs(position.z) > 200)
                {
                    log.Info($"Объект вышел за границы карты id={entity.id.value}");
                    entity.isDestroyed = true;
                }
            }
        }
    }
}