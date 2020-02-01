using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ParentsExtensions
{
    public static bool IsParentOf(this GameEntity e1, GameEntity e2, GameContext context)
    {
        var firstParent = e2;
        while (firstParent.hasParent)
        {
            if (firstParent.parent.id == e1.id.value) return true;
            firstParent = context.GetEntityWithId(firstParent.parent.id);
        }

        return false;
    }

    public static bool TryGetFirstGameEntity(this GameEntity entity, GameContext context, Predicate<GameEntity> predicate, out GameEntity result)
    {
        result = entity;
        while (result.hasParent)
        {
            if (predicate(result)) return true;
            result = context.GetEntityWithId(result.parent.id);
        }

        return predicate(result);
    }
}