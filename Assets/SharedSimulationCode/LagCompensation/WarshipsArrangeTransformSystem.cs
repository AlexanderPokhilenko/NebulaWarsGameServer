using Entitas;
using UnityEngine;

namespace SharedSimulationCode.LagCompensation
{
    /// <summary>
    /// Расставляет корабли согласно данным игрового состояния
    /// </summary>
    public class WarshipsArrangeTransformSystem : ArrangeTransformSystem
    {
        private readonly IGroup<GameEntity> warshipsGroup;

        public WarshipsArrangeTransformSystem(Contexts contexts)
        {
            warshipsGroup = contexts.game.GetGroup(GameMatcher
                .AllOf(GameMatcher.Transform, GameMatcher.HealthPoints));
        }
        
        public override void Execute()
        {
            foreach (var warship in warshipsGroup)
            {
                var isActive = GameState.transforms.ContainsKey(warship.id.value);
                if (!isActive)
                {
                    //выключить если этот объект не был создан
                    warship.transform.value.gameObject.SetActive(false);
                }
                else
                {
                    //передвинуть если этот объект был
                    Transform transform = GameState.transforms[warship.id.value];
                    warship.transform.value = transform;
                }
                //todo в игровом состоянии могли остаться объекты которые уже были уничтожены
                //с этим, кажется, ничего не нужно делать
            }
        }
    }
}