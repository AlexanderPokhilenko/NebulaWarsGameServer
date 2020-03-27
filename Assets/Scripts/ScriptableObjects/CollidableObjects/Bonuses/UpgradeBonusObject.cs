using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeBonus", menuName = "BaseObjects/Bonuses/UpgradeBonus", order = 53)]
public class UpgradeBonusObject : ActionBonusObject
{
    [Min(0)]
    public float percentage;

    protected override void Action(GameEntity entity)
    {
        if (entity.hasHealthPoints && entity.hasMaxHealthPoints)
        {
            var deltaHealth = entity.maxHealthPoints.value * percentage;

            entity.ReplaceHealthPoints(entity.healthPoints.value + deltaHealth);
            entity.ReplaceMaxHealthPoints(entity.maxHealthPoints.value + deltaHealth);
        }
    }
}
