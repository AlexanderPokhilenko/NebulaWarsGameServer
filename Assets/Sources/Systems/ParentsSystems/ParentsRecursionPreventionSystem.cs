﻿using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.Utils;
using UnityEngine;

public class ParentsRecursionPreventionSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext gameContext;

    public ParentsRecursionPreventionSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Parent);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasParent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var entityId = e.id.value;
            var firstParent = e;
            while (firstParent.hasParent)
            {
                firstParent = gameContext.GetEntityWithId(firstParent.parent.id);
                if (firstParent.id.value == entityId)
                {
                    Log.Warning("Parent recursion detected for entity with id " + entityId);
                    e.RemoveParent();
                    break;
                }
            }
        }
    }
}