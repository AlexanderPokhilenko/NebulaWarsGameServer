// //#define ParentAngularToLinear
// using UnityEngine;
//
// public static class CoordinatesExtensions
// {
//     private static readonly System.Random random = new System.Random();
//
//     public static Vector2 GetRandomUnitVector2()
//     {
//         var randomRadAngle = -Mathf.PI + 2 * (float)random.NextDouble() * Mathf.PI;
//         return new Vector2(Mathf.Cos(randomRadAngle), Mathf.Sin(randomRadAngle));
//     }
//
//     public static void ToGlobal(this ServerGameEntity entity, ServerGameContext context, out Vector2 position, out float angle, out int layer, out Vector2 velocity, out float angularVelocity)
//     {
//         layer = 0;
//         position = entity.position.value;
//         angle = entity.hasDirection ? entity.direction.angle : 0f;
//         velocity = entity.hasVelocity ? entity.velocity.value : Vector2.zero;
//         angularVelocity = entity.hasAngularVelocity ? entity.angularVelocity.value : 0f;
//
//         var firstParent = entity;
//         while (firstParent.hasParent)
//         {
//             firstParent = context.GetEntityWithId(firstParent.parent.id);
//             if (firstParent.hasAngularVelocity)
//             {
//                 if (position == Vector2.zero)
//                 {
//                     angularVelocity += firstParent.angularVelocity.value;
//                 }
// #if ParentAngularToLinear
//                 else
//                 {
//                     var leftPositionPerpendicular = new Vector2(-position.y, position.x);
//                     velocity += firstParent.angularVelocity.value * Mathf.Deg2Rad * leftPositionPerpendicular;
//                 }
// #endif
//             }
//             if (firstParent.hasDirection)
//             {
//                 var parentAngle = firstParent.direction.angle;
//                 GetSinCosFromDegrees(parentAngle, out var parentSin, out var parentCos);
//                 position.Rotate(parentSin, parentCos);
//                 velocity.Rotate(parentSin, parentCos);
//                 angle += parentAngle;
//             }
//             position += firstParent.position.value;
//             if (firstParent.hasVelocity) velocity += firstParent.velocity.value;
//             layer++;
//         }
//
//         while (angle >= 360f) angle -= 360f;
//     }
//
//     public static void Set(this ref Vector2 current, Vector2 other)
//     {
//         current.Set(other.x, other.y);
//     }
//
//     public static void Add(this ref Vector2 current, Vector2 other)
//     {
//         current.Set(current.x + other.x, current.y + other.y);
//     }
//
//     public static void Subtract(this ref Vector2 current, Vector2 other)
//     {
//         current.Set(current.x - other.x, current.y - other.y);
//     }
//
//     public static void Multiply(this ref Vector2 current, float coefficient)
//     {
//         current.Set(current.x * coefficient, current.y * coefficient);
//     }
//
//     public static void Divide(this ref Vector2 current, float coefficient)
//     {
//         var inverted = 1f / coefficient;
//         current.Multiply(inverted);
//     }
//
//     public static void ChangeSign(this ref Vector2 current)
//     {
//         current.Set(-current.x, -current.y);
//     }
//
//     public static Vector2 GetLocalVector(this ServerGameEntity entity, ServerGameContext context, Vector2 globalVector)
//     {
//         var currentAngle = entity.GetGlobalAngle(context);
//         if (entity.hasDirection) currentAngle -= entity.direction.angle;
//         return globalVector.GetRotated(-currentAngle);
//     }
//
//     public static Vector2 GetLocalRotatedVector(this ServerGameEntity entity, ServerGameContext context, Vector2 globalVector)
//     {
//         var currentAngle = entity.GetGlobalAngle(context);
//         return globalVector.GetRotated(-currentAngle);
//     }
//
//     public static float GetGlobalAngle(this ServerGameEntity entity, ServerGameContext context)
//     {
//         if (entity.hasGlobalTransform) return entity.globalTransform.angle;
//
//         var angle = entity.hasDirection ? entity.direction.angle : 0f;
//
//         var firstParent = entity;
//         while (firstParent.hasParent)
//         {
//             firstParent = context.GetEntityWithId(firstParent.parent.id);
//             if (firstParent.hasDirection)
//             {
//                 angle += firstParent.direction.angle;
//             }
//         }
//
//         while (angle >= 360f) angle -= 360f;
//
//         return angle;
//     }
//
//     public static Vector2 GetGlobalPositionVector2(this ServerGameEntity entity, ServerGameContext context)
//     {
//         if (entity.hasGlobalTransform) return entity.globalTransform.position;
//
//         var position = entity.position.value;
//
//         var firstParent = entity;
//         while (firstParent.hasParent)
//         {
//             firstParent = context.GetEntityWithId(firstParent.parent.id);
//             if (firstParent.hasDirection)
//             {
//                 var parentAngle = firstParent.direction.angle;
//                 GetSinCosFromDegrees(parentAngle, out var parentSin, out var parentCos);
//                 position.Rotate(parentSin, parentCos);
//             }
//             position += firstParent.position.value;
//         }
//         return position;
//     }
//
//     public static void GetSinCosFromDegrees(float angle, out float sin, out float cos)
//     {
//         sin = Mathf.Sin(angle * Mathf.Deg2Rad);
//         cos = Mathf.Cos(angle * Mathf.Deg2Rad);
//     }
//
//     public static Vector2 GetRotated(this Vector2 vector, float sin, float cos)
//     {
//         var newX = cos * vector.x - sin * vector.y;
//         var newY = sin * vector.x + cos * vector.y;
//         return new Vector2(newX, newY);
//     }
//
//     public static Vector2 GetRotated(this Vector2 vector, float angle)
//     {
//         GetSinCosFromDegrees(angle, out var sin, out var cos);
//         return vector.GetRotated(sin, cos);
//     }
//
//     public static Vector2 GetRotatedUnitVector2(float angle)
//     {
//         GetSinCosFromDegrees(angle, out var sin, out var cos);
//         return new Vector2(cos, sin);
//     }
//
//     public static void Rotate(this ref Vector2 vector, float sin, float cos)
//     {
//         var newX = cos * vector.x - sin * vector.y;
//         var newY = sin * vector.x + cos * vector.y;
//         vector.x = newX;
//         vector.y = newY;
//     }
//
//     public static void Rotate(this ref Vector2 vector, float angle)
//     {
//         GetSinCosFromDegrees(angle, out var sin, out var cos);
//         vector.Rotate(sin, cos);
//     }
//
//     public static Vector2[] GetRotatedVectors(Vector2[] vectors, float angle)
//     {
//         GetSinCosFromDegrees(angle, out var sin, out var cos);
//         var newVectors = new Vector2[vectors.Length];
//         for (var i = 0; i < vectors.Length; i++)
//         {
//             newVectors[i] = vectors[i].GetRotated(sin, cos);
//         }
//
//         return newVectors;
//     }
// }