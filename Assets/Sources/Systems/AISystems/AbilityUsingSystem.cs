using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AbilityUsingSystem : IExecuteSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> withAbilityGroup;
    private readonly System.Random random;
    private readonly List<GameEntity> buffer;
    private const int PredictedCapacity = 10;

    public AbilityUsingSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Target, GameMatcher.Ability, GameMatcher.Bot, GameMatcher.TargetingParameters).NoneOf(GameMatcher.AbilityCooldown);
        withAbilityGroup = gameContext.GetGroup(matcher);
        random = new System.Random();
        buffer = new List<GameEntity>(PredictedCapacity);
    }

    public void Execute()
    {
        foreach (var e in withAbilityGroup.GetEntities(buffer))
        {
            var currentPosition = e.GetGlobalPositionVector2(gameContext);

            var target = gameContext.GetEntityWithId(e.target.id);
            var targetPosition = target.GetGlobalPositionVector2(gameContext);

            var direction = targetPosition - currentPosition;
            var distance = direction.magnitude;

            // Используем ульту, только если цель достаточно близко
            if (distance >= e.targetingParameters.radius) continue;

            if (e.hasMaxHealthPoints)
            {
                var healthRatio = target.maxHealthPoints.value / e.maxHealthPoints.value;
                if (healthRatio < 1f)
                { // Если цель слабее нас, то уменьшаем вероятность активации ульты
                    var healthRatioPercentage = (int)(100f * healthRatio);
                    var rndGen = random.Next(100);
                    if(rndGen < healthRatioPercentage) continue;
                }
            }

            var maxVelocity = e.hasMaxVelocity ? e.maxVelocity.value : 1f; // TODO: учитывать скорость ульты
            var predictedTime = distance / maxVelocity;

            if (e.hasAngularVelocity)
            { // Смещаем направление, чтобы учесть угловую скорость
                direction.Rotate(e.angularVelocity.value * predictedTime);
            }

            var localTargetPosition = e.GetLocalRotatedVector(gameContext, direction);

            if (e.hasCannon) localTargetPosition -= e.cannon.position;

            var absProjectionY = Mathf.Abs(localTargetPosition.y);

            var relativeVelocity = Vector2.zero;
            if (target.hasVelocity) relativeVelocity += target.velocity.value;
            if (e.hasVelocity) relativeVelocity -= e.velocity.value;
            var localRelativeVelocity = e.GetLocalRotatedVector(gameContext, relativeVelocity);

            absProjectionY += localRelativeVelocity.y * predictedTime;

            if (absProjectionY <= target.circleCollider.radius) // Мы навелись на цель
            {
                e.ability.action(e, gameContext);
            }
        }
    }
}