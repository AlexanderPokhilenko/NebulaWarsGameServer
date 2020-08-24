// using UnityEngine;
//
// public abstract class EntityCreatorObject : ScriptableObject
// {
//     public abstract void FillEntity(ServerGameContext context, ServerGameEntity entity);
//
//     public void RefillEntity(ServerGameContext context, ServerGameEntity entity, Vector2 position, float direction)
//     {
//         var id = entity.id.value;
//
//         entity.RemoveAllComponents();
//         //entity.RemoveAllOnEntityReleasedHandlers();
//
//         entity.AddId(id);
//         entity.AddPosition(position);
//         entity.AddDirection(direction);
//
//         FillEntity(context, entity);
//     }
//
//     public ServerGameEntity CreateEntity(ServerGameContext context)
//     {
//         var entity = context.CreateEntity();
//         FillEntity(context, entity);
//         return entity;
//     }
//
//     public ServerGameEntity CreateEntity(ServerGameContext context, Vector2 position, float angle)
//     {
//         var entity = CreateEntity(context);
//         entity.AddPosition(position);
//         entity.AddDirection(angle);
//         if (entity.hasInitialDirectionSaver) entity.ReplaceInitialDirectionSaver(angle);
//         return entity;
//     }
//
//     public ServerGameEntity CreateEntity(ServerGameContext context, byte teamId)
//     {
//         var entity = CreateEntity(context);
//         foreach (var child in entity.GetAllChildrenGameEntities(context))
//         {
//             child.AddTeam(teamId);
//         }
//         return entity;
//     }
//
//     public ServerGameEntity CreateEntity(ServerGameContext context, Vector2 position, float angle, byte teamId)
//     {
//         var entity = CreateEntity(context, position, angle);
//         foreach (var child in entity.GetAllChildrenGameEntities(context))
//         {
//             child.AddTeam(teamId);
//         }
//         return entity;
//     }
// }
