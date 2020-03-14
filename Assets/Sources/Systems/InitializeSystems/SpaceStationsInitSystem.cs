using System;
using Entitas;
using UnityEngine;

public class SpaceStationsInitSystem : IInitializeSystem
{
    private static readonly BaseWithHealthObject spaceStation;
    private readonly GameContext gameContext;
    private const float Radius = 25f;
    private const int Count = 20;
    private const float Step = 360f / Count;

    static SpaceStationsInitSystem()
    {
        spaceStation = Resources.Load<BaseWithHealthObject>("SO/BaseObjects/SpaceStation");
        if (spaceStation == null)
            throw new Exception($"В {nameof(SpaceStationsInitSystem)} asset был null.");
    }

    public SpaceStationsInitSystem(Contexts contexts)
    {
        gameContext = contexts.game;
    }

    public void Initialize()
    {
        for (float angle = 0f; angle < 360f; angle += Step)
        {
            var position = CoordinatesExtensions.GetRotatedUnitVector2(angle) * Radius;

            var entity = spaceStation.CreateEntity(gameContext, position, angle);
        }
    }
}
