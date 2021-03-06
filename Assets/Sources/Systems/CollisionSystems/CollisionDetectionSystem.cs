﻿using Entitas;
using System.Collections.Generic;
using System.Linq;
using Server.GameEngine;
using Server.GameEngine.Chronometers;
using Server.GameEngine.Experimental;
using UnityEngine;

public sealed class CollisionDetectionSystem : IExecuteSystem, ICleanupSystem
{
    private readonly PositionChunks chunks;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> collidableGroup;
    private readonly List<GameEntity> buffer;
    private readonly Dictionary<ushort, int> idsToIndexes;
    private CollisionInfo[] collidables;
    private const int predictedCapacity = 512;

    public CollisionDetectionSystem(Contexts contexts, PositionChunks positionChunks)
    {
        chunks = positionChunks;
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Collidable, GameMatcher.CircleCollider, GameMatcher.Position);
        collidableGroup = gameContext.GetGroup(matcher);
        buffer = new List<GameEntity>(predictedCapacity);
        idsToIndexes = new Dictionary<ushort, int>(predictedCapacity);
        collidables = new CollisionInfo[predictedCapacity];
    }

    private struct CollisionInfo
    {
        public readonly ushort Id;
        public readonly GameEntity Entity;
        public readonly Vector2 GlobalPosition;
        public readonly float Radius;
        public readonly bool HasMass;
        public readonly float Mass;
        public readonly Vector2[] GlobalDots;
        public readonly Vector2[] GlobalAxises;
        public readonly bool IsIgnoringParentCollision;
        public readonly bool IsRound;
        public readonly bool IsPassingThrough;
        public readonly bool HasHealthPointsPart;
        public readonly GameEntity HealthPointsPart;
        public readonly bool HasBonusPickerPart;
        public readonly GameEntity BonusPickerPart;
        public readonly bool HasDamage;
        public readonly float Damage;
        public readonly ushort GrandOwnerId;
        public readonly ushort GrandParentId;
        public readonly byte TeamId;
        public readonly bool IsTargetingParasite;
        public readonly ushort GrandTargetId;
        public readonly bool HasBonus;
        public bool IsCollided;
        public bool WasProcessed;
        public Vector2 CollisionVector;

        public CollisionInfo(GameEntity entity, GameContext gameContext)
        {
            Id = entity.id.value;
            Entity = entity;
            Radius = entity.circleCollider.radius;
            if (entity.isUnmovable && !entity.hasParent)
            {
                HasMass = true;
                Mass = float.MaxValue / 2f;
            }
            else if (entity.hasMass)
            {
                HasMass = true;
                Mass = entity.mass.value;
            }
            else
            {
                HasMass = entity.TryGetFirstGameEntity(gameContext, part => part.hasMass, out var massPart);
                Mass = HasMass ? massPart.mass.value : 0f;
            }
            GlobalDots = entity.hasGlobalPathCollider ? entity.globalPathCollider.dots : null;
            GlobalAxises = entity.hasGlobalNoncollinearAxises ? entity.globalNoncollinearAxises.vectors : null;
            IsIgnoringParentCollision = entity.isIgnoringParentCollision;
            IsRound = entity.isRound;
            IsPassingThrough = entity.isPassingThrough;
            GlobalPosition = entity.hasGlobalTransform ? entity.globalTransform.position : entity.GetGlobalPositionVector2(gameContext);
            HasHealthPointsPart = entity.TryGetFirstGameEntity(gameContext, part => part.hasHealthPoints && !part.isInvulnerable, out HealthPointsPart);
            BonusPickerPart = null;
            HasBonusPickerPart = !IsPassingThrough && entity.TryGetFirstGameEntity(gameContext, part => part.isBonusPickable, out BonusPickerPart);
            HasDamage = entity.hasDamage;
            Damage = HasDamage ? (IsPassingThrough && !entity.isCollapses ? entity.damage.value * Chronometer.DeltaTime : entity.damage.value) : 0f;
            GrandOwnerId = entity.hasGrandOwner ? entity.grandOwner.id : Id;
            GrandParentId = entity.hasParent ? entity.GetGrandParent(gameContext).id.value : Id;
            TeamId = entity.hasTeam ? entity.team.id : byte.MaxValue;
            var hasTarget = entity.hasTarget;
            IsTargetingParasite = entity.isParasite && hasTarget;
            GrandTargetId = hasTarget ? gameContext.GetEntityWithId(entity.target.id).GetGrandParent(gameContext).id.value : (ushort)0;
            HasBonus = (entity.hasBonusAdder || entity.hasActionBonus) && !entity.hasBonusTarget;
            IsCollided = false;
            WasProcessed = false;
            CollisionVector = new Vector2(0f, 0f);
        }
    }

    public void Execute()
    {
        idsToIndexes.Clear();
        var entities = collidableGroup.GetEntities(buffer);
        var count = entities.Count;
        if (count > collidables.Length) collidables = new CollisionInfo[count];
        for (int i = 0; i < count; i++)
        {
            var entity = entities[i];
            collidables[i] = new CollisionInfo(entity, gameContext);
            idsToIndexes[collidables[i].Id] = i;
        }
        // ReSharper disable TooWideLocalVariableScope
        ref var current = ref collidables[0];
        GameEntity currentEntity;
        ref var other = ref collidables[0];
        GameEntity otherEntity;
        var distance = Vector2.zero;
        float closeDistance;
        float sqrDistance;
        bool collided;
        var penetration = Vector2.zero;
        // ReSharper restore TooWideLocalVariableScope
        foreach (var (currentId, otherId) in chunks.SameChunkPairs)
        {
            var currentIndex = idsToIndexes[currentId];
            var otherIndex = idsToIndexes[otherId];

            current = ref collidables[currentIndex];
            currentEntity = current.Entity;

            other = ref collidables[otherIndex];
            otherEntity = other.Entity;

            if(current.WasProcessed || other.WasProcessed) continue;

            if (other.TeamId == current.TeamId && // отключает friendly fire
                    (current.HasDamage || other.HasDamage || current.HasBonus || other.HasBonus)) continue;
            if ((other.IsIgnoringParentCollision || current.IsIgnoringParentCollision)
                && current.GrandParentId == other.GrandParentId) continue;

            distance.Set(other.GlobalPosition);
            distance.Subtract(current.GlobalPosition);
            closeDistance = other.Radius + current.Radius;
            sqrDistance = distance.sqrMagnitude;
            if (sqrDistance <= closeDistance * closeDistance)
            {
                if (current.IsRound)
                {
                    if (other.IsRound)
                    {
                        collided = true;
                        if (distance != Vector2.zero)
                        {
                            penetration.Set(distance);
                            penetration.Subtract(distance.normalized * closeDistance);
                        }
                        else
                        {
                            penetration.Set(CoordinatesExtensions.GetRandomUnitVector2() * closeDistance);
                        }
                    }
                    else
                    {
                        collided = CheckCollisionFigureWithCircle(other, current, out penetration);
                        penetration.ChangeSign();
                    }
                }
                else
                {
                    if (other.IsRound)
                    {
                        collided = CheckCollisionFigureWithCircle(current, other, out penetration);
                    }
                    else
                    {
                        collided = CheckCollisionFigureWithFigure(current, other, out penetration);
                    }
                }

                if (collided)
                {
                    if (!current.IsPassingThrough && !other.IsPassingThrough &&
                        !((current.IsTargetingParasite && current.GrandTargetId == other.GrandParentId) ||
                          (other.IsTargetingParasite && other.GrandTargetId == current.GrandParentId)))
                    {
                        current.IsCollided = true;
                        other.IsCollided = true;

                        if (current.HasMass || other.HasMass)
                        {
                            var totalMass = current.Mass + other.Mass;
                            current.CollisionVector.Add(penetration * (other.Mass / totalMass));
                            other.CollisionVector.Subtract(penetration * (current.Mass / totalMass));
                        }
                        else
                        {
                            current.CollisionVector.Add(penetration * 0.5f);
                            other.CollisionVector.Subtract(penetration * 0.5f);
                        }
                    }

                    if (current.HasDamage && !other.IsPassingThrough && other.HasHealthPointsPart)
                    {
                        current.IsCollided = true;
                        var otherHealthPointsPart = other.HealthPointsPart;
                        otherHealthPointsPart.ReplaceHealthPoints(otherHealthPointsPart.healthPoints.value - current.Damage);
                        if (otherHealthPointsPart.healthPoints.value <= 0 && !otherHealthPointsPart.hasKilledBy) otherHealthPointsPart.AddKilledBy(current.GrandOwnerId);
                    }
                    if (other.HasDamage && !current.IsPassingThrough && current.HasHealthPointsPart)
                    {
                        other.IsCollided = true;
                        var currentHealthPointsPart = current.HealthPointsPart;
                        currentHealthPointsPart.ReplaceHealthPoints(currentHealthPointsPart.healthPoints.value - other.Damage);
                        if (currentHealthPointsPart.healthPoints.value <= 0 && !currentHealthPointsPart.hasKilledBy) currentHealthPointsPart.AddKilledBy(other.GrandOwnerId);
                    }

                    if (current.HasBonus && !other.IsPassingThrough && other.HasBonusPickerPart)
                    {
                        if (currentEntity.hasBonusTarget)
                        {
                            current.WasProcessed = true;
                            continue;
                        };
                        var otherBonusPicker = other.BonusPickerPart;
                        if (currentEntity.hasBonusAdder && otherBonusPicker.GetAllChildrenGameEntities(gameContext, c => c.hasViewType && c.viewType.id == currentEntity.bonusAdder.bonusObject.typeId).Any()) continue;
                        if (currentEntity.hasActionBonus && !currentEntity.actionBonus.check(otherBonusPicker)) continue;
                        current.IsCollided = true;
                        currentEntity.AddBonusTarget(other.BonusPickerPart.id.value);
                        current.WasProcessed = true;
                        continue;
                    }
                    else if (other.HasBonus && !current.IsPassingThrough && current.HasBonusPickerPart)
                    {
                        if (otherEntity.hasBonusTarget)
                        {
                            other.WasProcessed = true;
                            continue;
                        };
                        var currentBonusPicker = current.BonusPickerPart;
                        if (otherEntity.hasBonusAdder && currentBonusPicker.GetAllChildrenGameEntities(gameContext, c => c.hasViewType && c.viewType.id == otherEntity.bonusAdder.bonusObject.typeId).Any()) continue;
                        if (otherEntity.hasActionBonus && !otherEntity.actionBonus.check(currentBonusPicker)) continue;
                        other.IsCollided = true;
                        otherEntity.AddBonusTarget(current.BonusPickerPart.id.value);
                        other.WasProcessed = true;
                        continue;
                    }
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            current = ref collidables[i];
            if (current.IsCollided)
            {
                currentEntity = current.Entity;
                currentEntity.isCollided = true;
                currentEntity.AddCollisionVector(current.CollisionVector);
            }
        }

        //collidables.Clear();
    }

    public void Cleanup()
    {
        foreach (var e in collidableGroup)
        {
            e.isCollided = false;
            if(e.hasCollisionVector) e.RemoveCollisionVector();
        }
    }

    private static bool CheckCollisionFigureWithCircle(CollisionInfo figure, CollisionInfo circle, out Vector2 penetration)
    {
        var circleRadius = circle.Radius;
        var circleGlobalPosition = circle.GlobalPosition;
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        var dots = figure.GlobalDots;
        var axises = figure.GlobalAxises;

        foreach (var axis in axises)
        {
            GetMinMaxOnAxis(dots, axis, out var min1, out var max1);
            var circleProjection = Vector2.Dot(circleGlobalPosition, axis);
            var min2 = circleProjection - circleRadius;
            var max2 = circleProjection + circleRadius;
            float depth;
            //первая фигура находится слева
            if (min1 < min2)
            {
                depth = max1 - min2;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetration.Set(depth * -axis);
                    prevDepth = depth;
                }
            }
            else
            {
                depth = max2 - min1;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetration.Set(depth * axis);
                    prevDepth = depth;
                }
            }
        }

        return true;
    }

    private static bool CheckCollisionFigureWithFigure(CollisionInfo figure1, CollisionInfo figure2, out Vector2 penetration)
    {
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        var axises1 = figure1.GlobalAxises;
        var dots1 = figure1.GlobalDots;
        var axises2 = figure2.GlobalAxises;
        var dots2 = figure2.GlobalDots;

        //возможно, стоит пропускать оси, между которыми очень малый угол

        foreach (var axis in axises1)
        {
            if (!CheckAxisPenetration(axis, ref penetration)) return false;
        }
        foreach (var axis in axises2)
        {
            if (!CheckAxisPenetration(axis, ref penetration)) return false;
        }

        return true;

        bool CheckAxisPenetration(Vector2 axis, ref Vector2 penetrationVector)
        {
            GetMinMaxOnAxis(dots1, axis, out var min1, out var max1);
            GetMinMaxOnAxis(dots2, axis, out var min2, out var max2);
            float depth;
            //первая фигура находится слева
            if (min1 < min2)
            {
                depth = max1 - min2;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetrationVector.Set(depth * -axis);
                    prevDepth = depth;
                }
            }
            else
            {
                depth = max2 - min1;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetrationVector.Set(depth * axis);
                    prevDepth = depth;
                }
            }

            return true;
        }
    }

    private static void GetMinMaxOnAxis(Vector2[] dots, Vector2 axis, out float min, out float max)
    {
        min = float.PositiveInfinity;
        max = float.NegativeInfinity;
        foreach (var dot in dots)
        {
            var value = Vector2.Dot(dot, axis);
            if (value < min) min = value;
            if (value > max) max = value;
        }
    }
}
