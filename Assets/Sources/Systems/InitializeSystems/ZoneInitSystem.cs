using Entitas;
using UnityEngine;

public class ZoneInitSystem : IInitializeSystem
{
    private readonly FlameCircleObject flameCircle;
    private readonly GameContext gameContext;

    public ZoneInitSystem(Contexts contexts, FlameCircleObject zone)
    {
        gameContext = contexts.game;
        flameCircle = zone;
    }

    public void Initialize()
    {
        var entity = flameCircle.CreateEntity(gameContext);
        entity.AddPosition(Vector2.zero);
        entity.AddDirection(0);
        gameContext.SetZone(entity.id.value);
    }
}
