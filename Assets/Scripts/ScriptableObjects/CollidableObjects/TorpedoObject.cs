using UnityEngine;

[CreateAssetMenu(fileName = "NewTorpedo", menuName = "BaseObjects/Torpedo", order = 59)]
public class TorpedoObject : BulletObject
{
    public float distance;
    [Min(0)]
    public float detectionRadius;
    public bool useAngularTargeting;
    public bool onlyPlayerTargeting = true;
    public bool targetChanging;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddChaser(distance);
        if(detectionRadius > 0f) entity.AddTargetingParameters(useAngularTargeting, detectionRadius, onlyPlayerTargeting);
        entity.isTargetChanging = targetChanging;

        return entity;
    }
}
