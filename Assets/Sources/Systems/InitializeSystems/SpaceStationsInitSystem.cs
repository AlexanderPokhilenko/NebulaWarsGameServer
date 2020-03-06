using System;
using Entitas;
using UnityEngine;

public class SpaceStationsInitSystem : IInitializeSystem
{
    private readonly BaseWithHealthObject spaceStation;
    private readonly GameContext gameContext;
    private const float Radius = 25f;
    private const int Count = 20;
    private const float Step = 360f / Count;

    public SpaceStationsInitSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        spaceStation = Resources.Load<BaseWithHealthObject>("SO/BaseObjects/SpaceStation");
        if (spaceStation == null)
            throw new Exception($"Не удалось загрузить ассет {nameof(spaceStation)}");
    }

    public void Initialize()
    {
        for (float angle = 0f; angle < 360f; angle += Step)
        {
            var position = Vector2.right.GetRotated(angle) * Radius;

            var entity = spaceStation.CreateEntity(gameContext, position, angle);
        }
    }
}
