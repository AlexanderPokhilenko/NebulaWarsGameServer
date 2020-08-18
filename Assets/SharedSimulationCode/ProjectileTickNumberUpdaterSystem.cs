using Entitas;

namespace SharedSimulationCode
{
    /// <summary>
    /// Сдвигает время в котором должны быть расположены противники для снаряда
    /// </summary>
    public class ProjectileTickNumberUpdaterSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> tickNumberGroup;

        public ProjectileTickNumberUpdaterSystem(Contexts contexts)
        {
            tickNumberGroup = contexts.game.GetGroup(GameMatcher.TickNumber);
        }

        public void Execute()
        {
            foreach (var entity in tickNumberGroup)
            {
                entity.tickNumber.value += 1;
            }
        }
    }
}