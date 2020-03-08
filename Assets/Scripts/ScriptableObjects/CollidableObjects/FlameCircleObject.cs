using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameCircle", menuName = "BaseObjects/FlameCircle", order = 58)]
public class FlameCircleObject : AuraObject
{
    public float scalingSpeed = -1.0f;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddCircleScaling(scalingSpeed);
        entity.AddTargetScaling(0f);
        entity.isNonstandardRadius = true;

        return entity;
    }
}
