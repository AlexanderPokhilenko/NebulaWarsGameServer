using System.Collections.Generic;
using Entitas;

public class DropSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> droppingGroup;
    private readonly List<GameEntity> buffer;
    private const int PredictedCapacity = 32;

    public DropSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Destroyed, GameMatcher.Drop, GameMatcher.Direction, GameMatcher.Position);
        droppingGroup = contexts.game.GetGroup(matcher);
        buffer = new List<GameEntity>(PredictedCapacity);
    }

    public void Execute()
    {
        foreach (var e in droppingGroup.GetEntities(buffer))
        {
            var hasSkin = e.hasSkin;
            var skin = hasSkin ? e.skin.value : null;
            var hasTeam = e.hasTeam;
            var team = hasTeam ? e.team.id : (byte)0;
            var hasAttackIncreasing = e.hasAttackIncreasing;
            var attackIncreasing = hasAttackIncreasing ? e.attackIncreasing.value : 0f;
            e.ToGlobal(gameContext, out var position, out var angle, out _, out var velocity, out var angularVelocity);
            var grandOwnerId = e.GetGrandOwnerId(gameContext);
            var hasTargetingParameters = e.hasTargetingParameters;
            var targetingParameters = hasTargetingParameters ? e.targetingParameters : null;
            var hasTarget = e.hasTarget;
            var target = hasTarget ? e.target.id : (ushort)0;

            var drops = e.drop.objects;
            for (var i = 1; i < drops.Count; i++)
            {
                var drop = drops[i];
                var dropEntity = drop.CreateEntity(gameContext, position, angle);
                SetupDrop(dropEntity);
            }

            drops[0].RefillEntity(gameContext, e, position, angle); // Переиспользование текущего объекта вместо удаления
            SetupDrop(e);

            void SetupDrop(GameEntity dropEntity)
            {
                // ReSharper disable once PossibleNullReferenceException
                if (hasSkin) skin.AddSkin(dropEntity, gameContext);
                if (hasAttackIncreasing)
                {
                    foreach (var dropChild in dropEntity.GetAllChildrenGameEntities(gameContext))
                    {
                        dropChild.AddAttackIncreasing(attackIncreasing);
                        if (dropChild.hasDamage) dropChild.ReplaceDamage(dropChild.damage.value * attackIncreasing);
                    }
                }

                if (hasTeam)
                {
                    foreach (var child in dropEntity.GetAllChildrenGameEntities(gameContext))
                    {
                        child.AddTeam(team);
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

                dropEntity.AddOwner(grandOwnerId);
                dropEntity.AddGrandOwner(grandOwnerId);

                if (dropEntity.hasChaser)
                {
                    if (hasTargetingParameters && !dropEntity.hasTargetingParameters)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        dropEntity.AddTargetingParameters(targetingParameters.angularTargeting,
                            targetingParameters.radius, targetingParameters.onlyPlayerTargeting);
                    }

                    if (hasTarget && dropEntity.hasTargetingParameters)
                    {
                        dropEntity.AddTarget(target);
                    }
                }
            }
        }
    }
}