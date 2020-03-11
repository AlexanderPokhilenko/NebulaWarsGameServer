using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using UnityEngine;


public static class CoordinatesExtensions
{
    public static Vector2 GetRandomUnitVector2()
    {
        float random = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

    public static void ToGlobal(this GameEntity entity, GameContext context, out Vector2 position, out float angle, out int layer, out Vector2 velocity, out float angularVelocity)
    {
        layer = 0;
        position = entity.position.value;
        angle = entity.hasDirection ? entity.direction.angle : 0f;
        velocity = entity.hasVelocity ? entity.velocity.value : Vector2.zero;
        angularVelocity = entity.hasAngularVelocity ? entity.angularVelocity.value : 0f;

        var firstParent = entity;
        while (firstParent.hasParent)
        {
            firstParent = context.GetEntityWithId(firstParent.parent.id);
            if (firstParent.hasAngularVelocity)
            {
                var leftPositionPerpendicular = new Vector2(-position.y, position.x);
                if (leftPositionPerpendicular != Vector2.zero)
                {
                    velocity += firstParent.angularVelocity.value * Mathf.Deg2Rad * leftPositionPerpendicular;
                }
                else
                {
                    angularVelocity += firstParent.angularVelocity.value;
                }
            }
            if (firstParent.hasDirection)
            {
                var parentAngle = firstParent.direction.angle;
                GetSinCosFromDegrees(parentAngle, out var parentSin, out var parentCos);
                position.Rotate(parentSin, parentCos);
                velocity.Rotate(parentSin, parentCos);
                angle += parentAngle;
            }
            position += firstParent.position.value;
            if (firstParent.hasVelocity) velocity += firstParent.velocity.value;
            layer++;
        }

        while (angle >= 360f) angle -= 360f;
    }

    public static Vector2 GetLocalVector(this GameEntity entity, GameContext context, Vector2 globalVector)
    {
        var currentAngle = entity.GetGlobalAngle(context);
        if (entity.hasDirection) currentAngle -= entity.direction.angle;
        return globalVector.GetRotated(-currentAngle);
    }

    public static Vector2 GetLocalRotatedVector(this GameEntity entity, GameContext context, Vector2 globalVector)
    {
        var currentAngle = entity.GetGlobalAngle(context);
        return globalVector.GetRotated(-currentAngle);
    }

    public static float GetGlobalAngle(this GameEntity entity, GameContext context)
    {
        if (entity.hasGlobalTransform) return entity.globalTransform.angle;

        var angle = entity.hasDirection ? entity.direction.angle : 0f;

        var firstParent = entity;
        while (firstParent.hasParent)
        {
            firstParent = context.GetEntityWithId(firstParent.parent.id);
            if (firstParent.hasDirection)
            {
                angle += firstParent.direction.angle;
            }
        }

        while (angle >= 360f) angle -= 360f;

        return angle;
    }

    public static Vector2 GetGlobalPositionVector2(this GameEntity entity, GameContext context)
    {
        if (entity.hasGlobalTransform) return entity.globalTransform.position;

        var position = entity.position.value;

        var firstParent = entity;
        while (firstParent.hasParent)
        {
            firstParent = context.GetEntityWithId(firstParent.parent.id);
            if (firstParent.hasDirection)
            {
                var parentAngle = firstParent.direction.angle;
                GetSinCosFromDegrees(parentAngle, out var parentSin, out var parentCos);
                position.Rotate(parentSin, parentCos);
            }
            position += firstParent.position.value;
        }
        return position;
    }

    public static void GetSinCosFromDegrees(float angle, out float sin, out float cos)
    {
        sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        cos = Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    public static Vector2 GetRotated(this Vector2 vector, float sin, float cos)
    {
        var newX = cos * vector.x - sin * vector.y;
        var newY = sin * vector.x + cos * vector.y;
        return new Vector2(newX, newY);
    }

    public static Vector2 GetRotated(this Vector2 vector, float angle)
    {
        GetSinCosFromDegrees(angle, out var sin, out var cos);
        return vector.GetRotated(sin, cos);
    }

    public static Vector2 GetRotatedUnitVector2(float angle)
    {
        GetSinCosFromDegrees(angle, out var sin, out var cos);
        return new Vector2(cos, sin);
    }

    public static void Rotate(this ref Vector2 vector, float sin, float cos)
    {
        var newX = cos * vector.x - sin * vector.y;
        var newY = sin * vector.x + cos * vector.y;
        vector.x = newX;
        vector.y = newY;
    }

    public static void Rotate(this ref Vector2 vector, float angle)
    {
        GetSinCosFromDegrees(angle, out var sin, out var cos);
        vector.Rotate(sin, cos);
    }

    public static Vector2[] GetRotatedVectors(Vector2[] vectors, float angle)
    {
        GetSinCosFromDegrees(angle, out var sin, out var cos);
        var newVectors = new Vector2[vectors.Length];
        for (var i = 0; i < vectors.Length; i++)
        {
            newVectors[i] = vectors[i].GetRotated(sin, cos);
        }

        return newVectors;
    }
}