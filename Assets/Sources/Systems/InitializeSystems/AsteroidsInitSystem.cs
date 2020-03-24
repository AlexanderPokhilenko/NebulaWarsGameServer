using System;
using System.Linq;
using Entitas;
using UnityEngine;

public class AsteroidsInitSystem : IInitializeSystem
{
    private static readonly (BaseWithHealthObject asset, int probability)[] asteroids;
    private static readonly int max;
    private readonly System.Random random;
    private readonly GameContext gameContext;

    static AsteroidsInitSystem()
    {
        asteroids = new (BaseWithHealthObject asset, int probability)[]
        {
            (Resources.Load<BaseWithHealthObject>("SO/BaseObjects/Asteroid100"), 10),
            (Resources.Load<BaseWithHealthObject>("SO/BaseObjects/Asteroid300"), 13),
            (Resources.Load<BaseWithHealthObject>("SO/BaseObjects/Asteroid300x200"), 17)
        };

        if(asteroids.Any(a => a.asset == null))
            throw new Exception($"В {nameof(AsteroidsInitSystem)} asset был null.");

        max = asteroids[asteroids.Length - 1].probability;
    }

    public AsteroidsInitSystem(Contexts contexts)
    {
        random = new System.Random();
        gameContext = contexts.game;
    }

    public void Initialize()
    {
        for (int i = 0; i < 250; i++)
        {
            var rndType = random.Next(max);
            var asteroid = asteroids.SkipWhile(a => a.probability < rndType).First().asset;

            var position = CoordinatesExtensions.GetRandomUnitVector2() * (25 + 5 * GetPositiveRandom(0.4f));

            var randomAngle = (float) random.NextDouble() * 360f;

            var entity = asteroid.CreateEntity(gameContext, position, randomAngle);
        }
    }

    private float GetPositiveRandom(float k)
    {
        var limitation = Mathf.PI / k;
        var rndX = 2 * limitation * (float)random.NextDouble() - limitation;
        return Mathf.Cos(k * rndX) + 1;
    }
}
