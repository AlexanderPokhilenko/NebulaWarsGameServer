using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class CannonShootRemoverSystem : ReactiveSystem<GameEntity>
{

    public CannonShootRemoverSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TryingToShoot.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isTryingToShoot;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            e.isTryingToShoot = false;
        }
    }
}