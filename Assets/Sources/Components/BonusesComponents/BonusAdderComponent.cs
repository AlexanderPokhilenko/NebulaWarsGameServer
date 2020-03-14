using Entitas;

[Game]
public sealed class BonusAdderComponent : IComponent
{
    public BaseObject bonusObject;
    public float duration;
    public bool colliderInheritance;
}
