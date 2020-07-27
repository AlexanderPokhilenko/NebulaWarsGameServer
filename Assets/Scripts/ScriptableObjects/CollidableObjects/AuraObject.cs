using UnityEngine;

[CreateAssetMenu(fileName = "NewAuraObject", menuName = "BaseObjects/AuraObject", order = 57)]
public class AuraObject : BaseObject
{
    public float angularVelocity;
    [Min(0)]
    public float outerRadius = float.PositiveInfinity;
    [Min(0)]
    public float damage;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddAngularVelocity(angularVelocity);
        entity.AddAura(outerRadius, damage);
        entity.isCollidable = false;
    }
}
