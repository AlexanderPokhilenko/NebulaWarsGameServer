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
            var hasSkin = e.hasSkin;
            var hasAttackIncreasing = e.hasAttackIncreasing;
            var attackIncreasing = hasAttackIncreasing ? e.attackIncreasing.value : 0f;
            e.ToGlobal(gameContext, out var position, out var angle, out _, out var velocity, out var angularVelocity);

            var drops = e.drop.objects;
            foreach (var drop in drops)
            {
                var dropEntity = drop.CreateEntity(gameContext, position, angle);
                if(hasSkin) e.skin.value.AddSkin(dropEntity, gameContext);
                if (hasAttackIncreasing)
                {
                    foreach (var dropChild in dropEntity.GetAllChildrenGameEntities(gameContext))
                    {
                        dropChild.AddAttackIncreasing(attackIncreasing);
                        if(dropChild.hasDamage) dropChild.ReplaceDamage(dropChild.damage.value * attackIncreasing);
                    }
                }
                if (e.hasTeam)
                {
                    foreach (var child in dropEntity.GetAllChildrenGameEntities(gameContext))
                    {
                        child.AddTeam(e.team.id);
                    }
                }
                dropEntity.AddGlobalTransform(position, angle);
                if ((dropEntity.hasActionBonus || dropEntity.hasBonusAdder) && dropEntity.hasCircleCollider)
                {
                    dropEntity.ReplaceDirection(0f);
                    dropEntity.ReplaceGlobalTransform(position, 0f);
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
                        dropEntity.AddTargetingParameters(e.targetingParameters.angularTargeting,
                            e.targetingParameters.radius, e.targetingParameters.onlyPlayerTargeting);
                    }

                    if (e.hasTarget && dropEntity.hasTargetingParameters)
                    {
                        dropEntity.AddTarget(e.target.id);
                    }
                }
            }
        }
    }
}