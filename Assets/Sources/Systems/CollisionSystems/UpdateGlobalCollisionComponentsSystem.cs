using Entitas;

public sealed class UpdateGlobalCollisionComponentsSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> collisionGroup;

    public UpdateGlobalCollisionComponentsSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.PathCollider, GameMatcher.NoncollinearAxises, GameMatcher.GlobalPathCollider, GameMatcher.GlobalNoncollinearAxises, GameMatcher.GlobalTransform);
        collisionGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in collisionGroup)
        {
            CoordinatesExtensions.GetSinCosFromDegrees(e.GetGlobalAngle(gameContext), out var sin, out var cos);
            
            var dots = e.pathCollider.dots;
            var globalDots = e.globalPathCollider.dots;
            for (var i = 0; i < dots.Length; i++)
            {
                globalDots[i] = dots[i].GetRotated(sin, cos) + e.GetGlobalPositionVector2(gameContext);
            }

            var axises = e.noncollinearAxises.vectors;
            var globalAxises = e.globalNoncollinearAxises.vectors;
            for (var i = 0; i < axises.Length; i++)
            {
                globalAxises[i] = axises[i].GetRotated(sin, cos);
            }
        }
    }
}
