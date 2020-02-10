using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "BaseObjects/Player", order = 55)]
public class PlayerObject : MovableWithHealthObject
{

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.isBonusPickable = true;
        entity.isSingleTargeting = true;

        return entity;
    }
}
