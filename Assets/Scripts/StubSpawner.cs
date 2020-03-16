#if UNITY_EDITOR
using Entitas;
using UnityEngine;

public class StubSpawner : MonoBehaviour
{
    private Systems systems;
    public BaseObject prototype;
    public Vector2 position;
    public float direction;
    public bool spawnOne;

    private GameContext gameContext;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Запущен {nameof(StubSpawner)}.");

        var contexts = Contexts.sharedInstance;
        contexts.SubscribeId();
        gameContext = contexts.game;

        systems = new Systems()
                .Add(new ParentsSystems(contexts))
                .Add(new AISystems(contexts))
                .Add(new MovementSystems(contexts))
                .Add(new GlobalTransformSystem(contexts))
                .Add(new ShootingSystems(contexts))
                .Add(new CollisionSystems(contexts))
                .Add(new EffectsSystems(contexts))
                .Add(new TimeSystems(contexts))
                .Add(new DestroySystems(contexts))
            ;

        systems.ActivateReactiveSystems();
        systems.Initialize();

        var collidersDrawer = FindObjectOfType<CollidersDrawer>();
        collidersDrawer.ChangeContext(gameContext);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnOne && prototype != null)
        {
            var entity = prototype.CreateEntity(gameContext, position, direction);
            if(entity.isNotDecelerating && entity.hasMaxVelocity && !entity.isUnmovable) entity.AddVelocity(CoordinatesExtensions.GetRotatedUnitVector2(direction));
            spawnOne = false;
        }

        systems.Execute();
        systems.Cleanup();
    }

    void OnDestroy()
    {
        systems.DeactivateReactiveSystems();
        systems.TearDown();
        systems.ClearReactiveSystems();
    }
}
#endif