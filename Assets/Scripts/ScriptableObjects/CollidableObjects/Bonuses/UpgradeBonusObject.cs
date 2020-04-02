using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeBonus", menuName = "BaseObjects/Bonuses/UpgradeBonus", order = 53)]
public class UpgradeBonusObject : ActionBonusObject
{
    [Min(0)]
    public float percentage;

    protected override bool Check(GameEntity entity)
    {
        return entity.hasHealthPoints && entity.hasMaxHealthPoints;
    }

    protected override void Action(GameEntity entity)
    {
        var deltaHealth = entity.maxHealthPoints.value * percentage;

        entity.ReplaceHealthPoints(entity.healthPoints.value + deltaHealth);
        entity.ReplaceMaxHealthPoints(entity.maxHealthPoints.value + deltaHealth);
    }
}
