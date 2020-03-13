using UnityEngine;

[CreateAssetMenu(fileName = "NewFighter", menuName = "BaseObjects/Fighter", order = 61)]
public class FighterObject : MovableWithHealthObject
{
    [Min(0)]
    public float detectionRadius;
    public bool useAngularTargeting;
    public bool onlyPlayerTargeting = true;
    public bool targetChanging;
    public float distance;
    public bool isParasite;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddTargetingParameters(useAngularTargeting, detectionRadius, onlyPlayerTargeting);
        entity.isTargetChanging = targetChanging;
        entity.AddChaser(distance);
        entity.isParasite = isParasite;

        return entity;
    }
}
