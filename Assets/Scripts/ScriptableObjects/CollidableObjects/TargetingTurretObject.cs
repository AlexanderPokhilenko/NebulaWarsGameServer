using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetingTurretObject", menuName = "BaseObjects/TargetingTurretObject", order = 56)]
public class TargetingTurretObject : BaseObject
{
    [Min(0)]
    public float maxAngularVelocity;
    [Min(0)]
    public float detectionRadius;
    public bool useAngularTargeting;
    public bool onlyPlayerTargeting = true;
    public bool targetChanging;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddMaxAngularVelocity(maxAngularVelocity);
        entity.AddTargetingParameters(useAngularTargeting, detectionRadius, onlyPlayerTargeting);
        entity.isTargetChanging = targetChanging;
        entity.AddInitialDirectionSaver(0f);
    }
}
