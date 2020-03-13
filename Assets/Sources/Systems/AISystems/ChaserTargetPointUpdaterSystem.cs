using Entitas;

public sealed class ChaserTargetPointUpdaterSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> chasingGroup;

    public ChaserTargetPointUpdaterSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Chaser, GameMatcher.Position, GameMatcher.Direction, GameMatcher.Target, GameMatcher.CircleCollider);
        chasingGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in chasingGroup)
        {
            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);
            var delta = targetPosition - e.GetGlobalPositionVector2(gameContext);

            var newPosition = targetPosition - delta.normalized * e.chaser.distance;

            if (e.hasTargetMovingPoint)
            {
                e.ReplaceTargetMovingPoint(newPosition);
            }
            else
            {
                e.AddTargetMovingPoint(newPosition);
            }
        }
    }
}
