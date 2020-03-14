﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewRepairBonus", menuName = "BaseObjects/Bonuses/RepairBonus", order = 52)]
public class RepairBonusObject : ActionBonusObject
{
    [Range(0, 1)]
    public float percentage;

    protected override void Action(GameEntity entity)
    {
        if (entity.hasHealthPoints && entity.hasMaxHealthPoints)
        {
            var newHealth = entity.maxHealthPoints.value * percentage + entity.healthPoints.value;

            if (newHealth > entity.maxHealthPoints.value) newHealth = entity.maxHealthPoints.value;

            entity.ReplaceHealthPoints(newHealth);
        }
    }
}