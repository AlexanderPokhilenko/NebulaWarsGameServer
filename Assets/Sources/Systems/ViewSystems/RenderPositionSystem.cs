using System.Collections.Generic;
using Entitas;

public class RenderPositionSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> positionedGroup;

    public RenderPositionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.View);
        positionedGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in positionedGroup)
        {
            e.view.gameObject.transform.position = e.GetGlobalPositionVector3(gameContext);
        }
    }
}