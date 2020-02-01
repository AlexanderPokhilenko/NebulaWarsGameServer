using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class RenderDirectionSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> directedGroup;

    public RenderDirectionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Direction, GameMatcher.View);
        directedGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in directedGroup)
        {
            e.view.gameObject.transform.rotation = Quaternion.AngleAxis(e.GetGlobalAngle(gameContext), Vector3.forward);
        }
    }
}