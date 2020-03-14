using System;
using System.Linq;
using Entitas;
using UnityEngine;

public class BonusesInitSystem : IInitializeSystem
{
    private static readonly BonusAdderObject[] bonuses;
    private static readonly float deltaStep;
    private const float Radius = 35f;
    private const int Count = 10;
    private const float Step = 360f / Count;
    private readonly GameContext gameContext;

    static BonusesInitSystem()
    {
        bonuses = new[]
        {
            Resources.Load<BonusAdderObject>("SO/Bonuses/PickableObjects/ShieldBonus"),
            Resources.Load<BonusAdderObject>("SO/Bonuses/PickableObjects/FireAuraBonus")
        };

        if (bonuses.Any(b => b == null))
            throw new Exception($"В {nameof(BonusesInitSystem)} asset был null.");

        deltaStep = Step / bonuses.Length;
    }

    public BonusesInitSystem(Contexts contexts)
    {
        gameContext = contexts.game;
    }

    public void Initialize()
    {
        for (float angle = 0f; angle < 360f; angle += Step)
        {
            for (int i = 0; i < bonuses.Length; i++)
            {
                var currentAngle = angle + i * deltaStep;

                var position = CoordinatesExtensions.GetRotatedUnitVector2(currentAngle) * Radius;

                bonuses[i].CreateEntity(gameContext, position, 0f);
            }
        }
    }
}
