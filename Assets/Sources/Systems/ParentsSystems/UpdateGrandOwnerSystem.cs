using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class UpdateGrandOwnerSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public UpdateGrandOwnerSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Owner);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasOwner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            var owner = gameContext.GetEntityWithId(e.owner.id);
            var newGrandOwnerId = owner != null && owner.hasGrandOwner ? owner.grandOwner.id : e.owner.id;
            var children = e.GetAllChildrenGameEntities(gameContext);
            foreach (var child in children)
            {
                if (child.hasGrandOwner)
                {
                    child.ReplaceGrandOwner(newGrandOwnerId);
                }
                else
                {
                    child.AddGrandOwner(newGrandOwnerId);
                }
            }
        }
    }
}