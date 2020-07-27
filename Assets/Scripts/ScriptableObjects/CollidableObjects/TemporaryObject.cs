using UnityEngine;

[CreateAssetMenu(fileName = "NewTemporaryObject", menuName = "BaseObjects/TemporaryObject", order = 59)]
public class TemporaryObject : BaseObject
{
    [Min(0)]
    public float lifetime = 1f;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddLifetime(lifetime);
    }
}
