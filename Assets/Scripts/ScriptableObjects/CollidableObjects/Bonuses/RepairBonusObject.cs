using UnityEngine;

[CreateAssetMenu(fileName = "NewRepairBonus", menuName = "BaseObjects/Bonuses/RepairBonus", order = 52)]
public class RepairBonusObject : BaseObject
{
    [Range(0, 1)]
    public float percentage;

    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddActionBonus(Repair);

        return entity;
    }

    private void Repair(GameEntity entity)
    {
        if (entity.hasHealthPoints && entity.hasMaxHealthPoints)
        {
            var newHealth = entity.maxHealthPoints.value * percentage + entity.healthPoints.value;

            if (newHealth > entity.maxHealthPoints.value) newHealth = entity.maxHealthPoints.value;

            entity.ReplaceHealthPoints(newHealth);
        }
    }
}
