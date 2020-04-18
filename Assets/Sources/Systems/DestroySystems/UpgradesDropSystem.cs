using Entitas;

public class UpgradesDropSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> droppingGroup;

    public UpgradesDropSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Destroyed, GameMatcher.Upgrades, GameMatcher.Direction, GameMatcher.Position);
        droppingGroup = contexts.game.GetGroup(matcher);
    }

    public void Execute()
    {
        foreach (var e in droppingGroup)
        {
            e.ToGlobal(gameContext, out var position, out var angle, out _, out var velocity, out var angularVelocity);

            var dropDictionary = e.upgrades.bonuses;
            foreach (var pair in dropDictionary)
            {
                var drop = pair.Key;
                var count = pair.Value;

                for (byte i = 0; i < count; i++)
                {
                    var dropEntity = drop.CreateEntity(gameContext, position, angle);
                    if ((dropEntity.hasActionBonus || dropEntity.hasBonusAdder) && dropEntity.hasCircleCollider)
                    {
                        dropEntity.ReplaceDirection(angle = 0f);
                    }
                    else
                    {
                        dropEntity.AddVelocity(velocity);
                        dropEntity.AddAngularVelocity(angularVelocity);
                    }

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

                    dropEntity.AddGlobalTransform(position, angle);
                }
            }
        }
    }
}