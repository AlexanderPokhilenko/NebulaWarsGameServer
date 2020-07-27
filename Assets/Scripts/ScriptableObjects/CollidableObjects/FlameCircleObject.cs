using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameCircle", menuName = "BaseObjects/FlameCircle", order = 58)]
public class FlameCircleObject : AuraObject
{
    public float scalingSpeed = -1.0f;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddCircleScaling(scalingSpeed);
        entity.AddTargetScaling(0f);
        entity.isNonstandardRadius = true;
    }
}
