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
    [Min(0)]
    public float maxHealthPoints;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddChaser(distance);
        if(detectionRadius > 0f) entity.AddTargetingParameters(useAngularTargeting, detectionRadius, onlyPlayerTargeting);
        entity.isTargetChanging = targetChanging;
        if (maxHealthPoints > 0)
        {
            entity.AddHealthPoints(maxHealthPoints);
            entity.AddMaxHealthPoints(maxHealthPoints);
            entity.isOnHealthDropDestroying = true;
        }
    }
}
