using UnityEngine;

public abstract class EntityCreatorObject : ScriptableObject
{
    public abstract GameEntity CreateEntity(GameContext context);

    public GameEntity CreateEntity(GameContext context, Vector2 position, float angle)
    {
        var entity = CreateEntity(context);
        entity.AddPosition(position);
        entity.AddDirection(angle);
        if (entity.hasInitialDirectionSaver) entity.ReplaceInitialDirectionSaver(angle);
        return entity;
    }

    public GameEntity CreateEntity(GameContext context, byte teamId)
    {
        var entity = CreateEntity(context);
        foreach (var child in entity.GetAllChildrenGameEntities(context))
        {
            child.AddTeam(teamId);
        }
        return entity;
    }

    public GameEntity CreateEntity(GameContext context, Vector2 position, float angle, byte teamId)
    {
        var entity = CreateEntity(context, position, angle);
        foreach (var child in entity.GetAllChildrenGameEntities(context))
        {
            child.AddTeam(teamId);
        }
        return entity;
    }
}
