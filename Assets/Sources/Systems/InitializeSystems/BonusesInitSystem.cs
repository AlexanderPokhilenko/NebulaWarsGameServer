using System;
using Entitas;
using UnityEngine;

public class BonusesInitSystem : IInitializeSystem
{
    private readonly BonusAdderObject[] bonuses;
    private readonly GameContext gameContext;
    private readonly float deltaStep;
    private const float Radius = 35f;
    private const int Count = 10;
    private const float Step = 360f / Count;

    public BonusesInitSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        bonuses = new[]
        {
            Resources.Load<BonusAdderObject>("SO/Bonuses/PickableObjects/ShieldBonus"),
            Resources.Load<BonusAdderObject>("SO/Bonuses/PickableObjects/FireAuraBonus")
        };
        deltaStep = Step / bonuses.Length;
    }

    public void Initialize()
    {
        for (float angle = 0f; angle < 360f; angle += Step)
        {
            for (int i = 0; i < bonuses.Length; i++)
            {
                var currentAngle = angle + i * deltaStep;
                var entity = bonuses[i].CreateEntity(gameContext);

                var position = Vector2.right.GetRotated(currentAngle) * Radius;

                entity.AddPosition(position);
                entity.AddDirection(0);
            }
        }
    }
}
