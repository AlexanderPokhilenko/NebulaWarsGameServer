using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Server.GameEngine;
using UnityEngine;

public sealed class CollisionDetectionSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private IGroup<GameEntity> collidableGroup;

    public CollisionDetectionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Collidable, GameMatcher.CircleCollider, GameMatcher.Position);
        collidableGroup = gameContext.GetGroup(matcher);
    }

    public void Execute()
    {
        var count = collidableGroup.count;
        if (count < 2) return;
        for (int i = 1; i < count; i++)
        {
            var current = collidableGroup.AsEnumerable().ElementAt(i - 1);
            var currentGlobalPosition = current.GetGlobalPositionVector2(gameContext);
            var currentPartHasHealthPoints = current.TryGetFirstGameEntity(gameContext, part => part.hasHealthPoints && !part.isInvulnerable, out var currentHealthPart);
            var currentPartCanPickBonuses = current.TryGetFirstGameEntity(gameContext, part => part.isBonusPickable, out var currentBonusPickerPart);
            var currentDamage = current.hasDamage ? (current.isPassingThrough && !current.isCollapses ? current.damage.value * Clock.deltaTime : current.damage.value) : 0f;
            var currentGrandOwner = current.GetGrandOwner(gameContext);
            var remaining = collidableGroup.AsEnumerable().Skip(i);
            foreach (var e in remaining)
            {
                //TODO: возможно, стоит убрать эту проверку
                if(e.GetGrandOwner(gameContext).id.value == currentGrandOwner.id.value) continue;
                if((e.isIgnoringParentCollision || current.isIgnoringParentCollision) &&
                   (e.IsParentOf(current, gameContext) || current.IsParentOf(e, gameContext))) continue;
                var distance = e.GetGlobalPositionVector2(gameContext) - currentGlobalPosition;
                var closeDistance = e.circleCollider.radius + current.circleCollider.radius;
                var sqrDistance = Vector2.SqrMagnitude(distance);
                if (sqrDistance <= closeDistance * closeDistance)
                {
                    bool collided;
                    Vector2 penetration;
                    if (current.isRound)
                    {
                        if (e.isRound)
                        {
                            collided = true;
                            if (distance != Vector2.zero)
                            {
                                penetration = distance - distance.normalized * closeDistance;
                            }
                            else
                            {
                                penetration = CoordinatesExtensions.GetRandomUnitVector2() * closeDistance;
                            }
                        }
                        else
                        {
                            collided = CheckCollisionFigureWithCircle(e, current, out penetration);
                            penetration = -penetration;
                        }
                    }
                    else
                    {
                        if (e.isRound)
                        {
                            collided = CheckCollisionFigureWithCircle(current, e, out penetration);
                        }
                        else
                        {
                            collided = CheckCollisionFigureWithFigure(current, e, out penetration);
                        }
                    }

                    //TODO: refactoring
                    if (collided)
                    {
                        e.isCollided = true;
                        current.isCollided = true;
                        if (!current.isPassingThrough && !e.isPassingThrough)
                        {
                            if (current.hasCollisionVector)
                            {
                                var newCCollVec = current.collisionVector.value + penetration;
                                current.ReplaceCollisionVector(newCCollVec);
                            }
                            else
                            {
                                current.AddCollisionVector(penetration);
                            }
                            if (e.hasCollisionVector)
                            {
                                var newECollVec = e.collisionVector.value - penetration;
                                e.ReplaceCollisionVector(newECollVec);
                            }
                            else
                            {
                                e.AddCollisionVector(-penetration);
                            }
                        }

                        if (current.hasDamage && e.TryGetFirstGameEntity(gameContext, part => part.hasHealthPoints && !part.isInvulnerable, out var ePart))
                        {
                            ePart.ReplaceHealthPoints(ePart.healthPoints.value - currentDamage);
                            if(ePart.healthPoints.value <= 0 && !ePart.hasKilledBy) ePart.AddKilledBy(currentGrandOwner.id.value);
                        }
                        if (e.hasDamage && currentPartHasHealthPoints)
                        {
                            var eDamage = e.isPassingThrough && !e.isCollapses ? e.damage.value * Clock.deltaTime : e.damage.value;
                            currentHealthPart.ReplaceHealthPoints(currentHealthPart.healthPoints.value - eDamage);
                            if (currentHealthPart.healthPoints.value <= 0 && !currentHealthPart.hasKilledBy) currentHealthPart.AddKilledBy(e.GetGrandOwner(gameContext).id.value);
                        }

                        if (current.hasBonusAdder && !current.hasBonusTarget && e.TryGetFirstGameEntity(gameContext, part => part.isBonusPickable, out var eBonusPickerPart))
                        {
                            current.AddBonusTarget(eBonusPickerPart.id.value);
                        }
                        else if (e.hasBonusAdder && !e.hasBonusTarget && currentPartCanPickBonuses)
                        {
                            e.AddBonusTarget(currentBonusPickerPart.id.value);
                        }
                    }
                }
            }
        }
    }

    public void Cleanup()
    {
        foreach (var e in collidableGroup)
        {
            e.isCollided = false;
            if(e.hasCollisionVector) e.RemoveCollisionVector();
        }
    }

    private bool CheckCollisionFigureWithCircle(GameEntity figure, GameEntity circle, out Vector2 penetration)
    {
        var figureGlobalPosition = figure.GetGlobalPositionVector2(gameContext);
        var circleGlobalPosition = circle.GetGlobalPositionVector2(gameContext);
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        GetFigureRotatedAxisesAndDots(figure, out var axises, out var dots);
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] += figureGlobalPosition;
        }

        foreach (var axis in axises)
        {
            GetMinMaxOnAxis(dots, axis, out var min1, out var max1);
            var circleProjection = Vector2.Dot(circleGlobalPosition, axis);
            var min2 = circleProjection - circle.circleCollider.radius;
            var max2 = circleProjection + circle.circleCollider.radius;
            float depth;
            //первая фигура находится слева
            if (min1 < min2)
            {
                depth = max1 - min2;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetration = -depth * axis;
                    prevDepth = depth;
                }
            }
            else
            {
                depth = max2 - min1;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetration = depth * axis;
                    prevDepth = depth;
                }
            }
        }

        return true;
    }

    private bool CheckCollisionFigureWithFigure(GameEntity figure1, GameEntity figure2, out Vector2 penetration)
    {
        var figure1GlobalPosition = figure1.GetGlobalPositionVector2(gameContext);
        var figure2GlobalPosition = figure2.GetGlobalPositionVector2(gameContext);
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        GetFigureRotatedAxisesAndDots(figure1, out var axises1, out var dots1);
        GetFigureRotatedAxisesAndDots(figure2, out var axises2, out var dots2);
        for (int i = 0; i < dots1.Length; i++)
        {
            dots1[i] += figure1GlobalPosition;
        }
        for (int i = 0; i < dots2.Length; i++)
        {
            dots2[i] += figure2GlobalPosition;
        }

        //возможно, стоит переписать участок с HashSet
        var axises = new HashSet<Vector2>(axises1);
        axises.UnionWith(axises2);

        foreach (var axis in axises)
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
                    penetration = -depth * axis;
                    prevDepth = depth;
                }
            }
            else
            {
                depth = max2 - min1;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetration = depth * axis;
                    prevDepth = depth;
                }
            }
        }

        return true;
    }

    private void GetFigureRotatedAxisesAndDots(GameEntity figure, out Vector2[] axises, out Vector2[] dots)
    {
        var globalAngle = figure.GetGlobalAngle(gameContext);
        axises = GetRotatedVectors(figure.noncollinearAxises.vectors, globalAngle);
        dots = GetRotatedVectors(figure.pathCollider.dots, globalAngle);
    }

    private void GetMinMaxOnAxis(Vector2[] dots, Vector2 axis, out float min, out float max)
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

    private Vector2[] GetRotatedVectors(Vector2[] vectors, float angle)
    {
        CoordinatesExtensions.GetSinCosFromDegrees(angle, out var sin, out var cos);
        var newVecs = new Vector2[vectors.Length];
        for (int i = 0; i < vectors.Length; i++)
        {
            newVecs[i] = vectors[i].GetRotated(sin, cos);
        }

        return newVecs;
    }
}
