using Entitas;
using System;

public static class ContextsIdExtensions
{
    public static void SubscribeId(this Contexts contexts)
    {
        foreach (var context in contexts.allContexts)
        {
            if (Array.FindIndex(context.contextInfo.componentTypes, v => v == typeof(IdComponent)) >= 0)
            {
                context.OnEntityCreated += AddId;
            }
        }
    }
    public static void UnsubscribeId(this Contexts contexts)
    {
        foreach (var context in contexts.allContexts)
        {
            if (Array.FindIndex(context.contextInfo.componentTypes, v => v == typeof(IdComponent)) >= 0)
            {
                context.OnEntityCreated -= AddId;
            }
        }

        contexts.Reset();
    }

    public static void AddId(IContext context, IEntity entity)
    {
        ((IIdEntity)entity).ReplaceId((ushort)entity.creationIndex);
    }
}