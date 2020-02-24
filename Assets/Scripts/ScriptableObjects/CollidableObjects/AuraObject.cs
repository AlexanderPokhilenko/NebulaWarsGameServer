using UnityEngine;

[CreateAssetMenu(fileName = "NewAuraObject", menuName = "BaseObjects/AuraObject", order = 57)]
public class AuraObject : BaseObject
{
    public float angularVelocity;
    [Min(0)]
    public float outerRadius = float.PositiveInfinity;
    [Min(0)]
    public float damage;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddAngularVelocity(angularVelocity);
        entity.AddAura(outerRadius, damage);
        entity.isCollidable = false;

        return entity;
    }
}
