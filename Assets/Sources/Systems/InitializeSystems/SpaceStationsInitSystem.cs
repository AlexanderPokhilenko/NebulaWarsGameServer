using Entitas;
using UnityEditor;
using UnityEngine;

public class SpaceStationsInitSystem : IInitializeSystem
{
    private readonly BaseWithHealthObject spaceStation;
    private readonly GameContext gameContext;
    private const int count = 20;
    private const float step = 360f / count;

    public SpaceStationsInitSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        //TODO: возможно, попробовать обойтись без AssetDatabase
        spaceStation = AssetDatabase.LoadAssetAtPath<BaseWithHealthObject>("Assets/SO/BaseObjects/SpaceStation.asset");
    }

    public void Initialize()
    {
        for (float angle = 0f; angle < 360f; angle += step)
        {
            var entity = spaceStation.CreateEntity(gameContext);

            var position = Vector2.right.GetRotated(angle) * 25;

            entity.AddPosition(position);
            entity.AddDirection(angle);
        }
    }
}
