using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "BaseObjects/Player", order = 55)]
public class PlayerObject : MovableWithHealthObject
{
    public AbilityInfo ability;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        ability.AddAbilityToEntity(entity);
        entity.isBonusPickable = true;
        entity.isSingleTargeting = true;

        return entity;
    }
}
