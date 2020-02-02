using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Systems systems;

    private void Awake()
    {
        var contexts = Contexts.sharedInstance;
        contexts.SubscribeId();

        systems = new Systems()
            .Add(new ParentsSystems(contexts))
            .Add(new MovementSystems(contexts))
            .Add(new ShootingSystems(contexts))
            .Add(new CollisionSystems(contexts))
            .Add(new EffectsSystems(contexts))
            .Add(new TimeSystems(contexts))
            .Add(new DestroySystems(contexts))
            .Add(new AISystems(contexts));

        systems.Initialize();
    }

    private void Update()
    {
        systems.Execute();
        systems.Cleanup();
    }

    private void OnDestroy()
    {
        systems.TearDown();
    }
}