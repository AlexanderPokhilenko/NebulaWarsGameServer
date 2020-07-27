using UnityEngine;

[CreateAssetMenu(fileName = "NewMovableHealthObject", menuName = "BaseObjects/MovableHealthObject", order = 54)]
public class MovableWithHealthObject : MovableObject
{
    [Min(0)]
    public float maxHealthPoints;

    public override void FillEntity(GameContext context, GameEntity entity)
    {
        base.FillEntity(context, entity);
        entity.AddHealthPoints(maxHealthPoints);
        entity.AddMaxHealthPoints(maxHealthPoints);
    }
}
