using Entitas;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Система по GameState расставляет GameObject-ы какого-то типа по местам.
    /// </summary>
    public abstract class ArrangeTransformSystem : IExecuteSystem
    {
        public GameState GameState { get; set; }
        public abstract void Execute();
    }
}