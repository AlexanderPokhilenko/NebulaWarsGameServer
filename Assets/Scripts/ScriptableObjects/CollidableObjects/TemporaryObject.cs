using UnityEngine;

[CreateAssetMenu(fileName = "NewTemporaryObject", menuName = "BaseObjects/TemporaryObject", order = 59)]
public class TemporaryObject : BaseObject
{
    [Min(0)]
    public float lifetime = 1f;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddLifetime(lifetime);

        return entity;
    }
}
