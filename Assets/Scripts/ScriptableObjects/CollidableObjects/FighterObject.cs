using UnityEngine;

[CreateAssetMenu(fileName = "NewFighter", menuName = "BaseObjects/Fighter", order = 60)]
public class FighterObject : MovableWithHealthObject
{
    [Min(0)]
    public float detectionRadius;
    public bool useAngularTargeting;
    public bool onlyPlayerTargeting = true;
    public bool targetChanging;
    public float distance;
    public bool isParasite;
    [Min(0)]
    public float lifetime;
    public bool useBotAI;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddTargetingParameters(useAngularTargeting, detectionRadius, onlyPlayerTargeting);
        entity.isTargetChanging = targetChanging;
        entity.AddChaser(distance);
        entity.isParasite = isParasite;
        if(lifetime > 0) entity.AddLifetime(lifetime);
        entity.isBot = useBotAI;

        return entity;
    }
}
