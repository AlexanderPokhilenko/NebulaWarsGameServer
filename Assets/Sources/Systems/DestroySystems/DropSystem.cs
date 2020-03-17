using Entitas;

public class DropSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> droppingGroup;

    public DropSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Destroyed, GameMatcher.Drop, GameMatcher.Direction, GameMatcher.Position);
        droppingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in droppingGroup)
        {
            e.ToGlobal(gameContext, out var position, out var angle, out _, out var velocity, out var angularVelocity);

            var drop = e.drop.value;
            var dropEntity = drop.CreateEntity(gameContext, position, angle);
            if((dropEntity.hasActionBonus || dropEntity.hasBonusAdder) && dropEntity.hasCircleCollider) dropEntity.ReplaceDirection(angle = 0f);

            var grandOwnerId = e.GetGrandOwnerId(gameContext);
            dropEntity.AddOwner(grandOwnerId);
            dropEntity.AddGrandOwner(grandOwnerId);

            if (dropEntity.hasChaser)
            {
                if (e.hasTargetingParameters && !dropEntity.hasTargetingParameters)
                {
                    dropEntity.AddTargetingParameters(e.targetingParameters.angularTargeting, e.targetingParameters.radius, e.targetingParameters.onlyPlayerTargeting);
                }

                if (e.hasTarget && dropEntity.hasTargetingParameters)
                {
                    dropEntity.AddTarget(e.target.id);
                }
            }

            dropEntity.AddVelocity(velocity);
            dropEntity.AddAngularVelocity(angularVelocity);

            dropEntity.AddGlobalTransform(position, angle);
        }
    }
}