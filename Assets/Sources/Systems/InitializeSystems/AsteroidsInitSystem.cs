using System;
using System.Linq;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidsInitSystem : IInitializeSystem
{
    private static readonly (BaseWithHealthObject asset, int probability)[] asteroids;
    private static readonly int max;
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
        gameContext = contexts.game;
        //TODO: что-то с этим сделать
        // asteroids = new (BaseWithHealthObject asset, int probability)[]
            
        // {
        //     (AssetDatabase.LoadAssetAtPath<BaseWithHealthObject>("Assets/SO/BaseObjects/Asteroid100.asset"), 10),
        //     (AssetDatabase.LoadAssetAtPath<BaseWithHealthObject>("Assets/SO/BaseObjects/Asteroid300.asset"), 13),
        //     (AssetDatabase.LoadAssetAtPath<BaseWithHealthObject>("Assets/SO/BaseObjects/Asteroid300x200.asset"), 17)
        // };

        // Resources.Load<PlayerObject>("SO/BaseObjects/HarePlayer");

    }

    public void Initialize()
    {
        for (int i = 0; i < 250; i++)
        {
            var rndType = Random.Range(0, max);
            var asteroid = asteroids.SkipWhile(a => a.probability < rndType).First().asset;

            var position = CoordinatesExtensions.GetRandomUnitVector2() * (25 + 5 * GetPositiveRandom(0.4f));

            var entity = asteroid.CreateEntity(gameContext, position, Random.Range(0f, 360f));
        }
    }

    private float GetPositiveRandom(float k)
    {
        var limitation = Mathf.PI / k;
        var rndX = Random.Range(-limitation, limitation);
        return Mathf.Cos(k * rndX) + 1;
    }
}
