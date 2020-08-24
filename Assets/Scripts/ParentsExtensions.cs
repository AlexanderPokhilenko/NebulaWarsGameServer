// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
//
// public static class ParentsExtensions
// {
//     public static bool IsParentOf(this ServerGameEntity e1, ServerGameEntity e2, ServerGameContext context)
//     {
//         var firstParent = e2;
//         while (firstParent.hasParent)
//         {
//             if (firstParent.parent.id == e1.id.value) return true;
//             firstParent = context.GetEntityWithId(firstParent.parent.id);
//         }
//
//         return false;
//     }
//
//     public static ServerGameEntity GetGrandParent(this ServerGameEntity entity, ServerGameContext context)
//     {
//         var firstParent = entity;
//         while (firstParent.hasParent)
//         {
//             firstParent = context.GetEntityWithId(firstParent.parent.id);
//         }
//
//         return firstParent;
//     }
//
//     public static ushort GetGrandOwnerId(this ServerGameEntity entity, ServerGameContext context)
//     {
//         if (entity.hasGrandOwner) return entity.grandOwner.id;
//         ushort result = entity.GetGrandParent(context).id.value;
//         var currentEntity = context.GetEntityWithId(result);
//         while (currentEntity != null && currentEntity.hasOwner)
//         {
//             result = currentEntity.owner.id;
//             currentEntity = context.GetEntityWithId(result);
//         }
//
//         return result;
//     }
//
//     public static bool TryGetFirstServerGameEntity(this ServerGameEntity entity, ServerGameContext context, 
//         Predicate<ServerGameEntity> predicate, out ServerGameEntity result)
//     {
//         result = entity;
//         while (result.hasParent)
//         {
//             if (predicate(result)) return true;
//             result = context.GetEntityWithId(result.parent.id);
//         }
//
//         return predicate(result);
//     }
//
//     public static IEnumerable<ServerGameEntity> GetAllChildrenGameEntities(this ServerGameEntity entity, ServerGameContext context, Predicate<ServerGameEntity> predicate)
//     {
//         if (predicate(entity)) yield return entity;
//         var children = context.GetEntitiesWithParent(entity.id.value);
//         foreach (var childServerGameEntity in children.SelectMany(child => GetAllChildrenGameEntities(child, context, predicate)))
//         {
//             yield return childServerGameEntity;
//         }
//     }
//
//     public static IEnumerable<ServerGameEntity> GetAllChildrenGameEntities(this ServerGameEntity entity, ServerGameContext context)
//     {
//         yield return entity;
//         var children = context.GetEntitiesWithParent(entity.id.value);
//         foreach (var childServerGameEntity in children.SelectMany(child => GetAllChildrenGameEntities(child, context)))
//         {
//             yield return childServerGameEntity;
//         }
//     }
//
//     public static IEnumerable<ServerGameEntity> GetCannons(this ServerGameEntity entity, ServerGameContext context)
//     {
//         return entity.hasSpecialShooter
//             ? entity.specialShooter.value.GetCannons(entity, context)
//             : entity.GetAllChildrenGameEntities(context, e => e.hasCannon);
//     }
// }