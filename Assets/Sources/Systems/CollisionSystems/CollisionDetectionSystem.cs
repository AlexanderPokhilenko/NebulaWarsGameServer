using Entitas;
using Server.GameEngine;
using System.Collections.Generic;
using UnityEngine;

public sealed class CollisionDetectionSystem : IExecuteSystem, ICleanupSystem
{
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> collidableGroup;
    private readonly List<GameEntity> buffer;

    public CollisionDetectionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Collidable, GameMatcher.CircleCollider, GameMatcher.Position);
        collidableGroup = gameContext.GetGroup(matcher);
        buffer = new List<GameEntity>(350);
    }

    public void Execute()
    {
        var entities = collidableGroup.GetEntities(buffer);
        var count = entities.Count;
        if (count < 2) return;
        for (int i = 1; i < count; i++)
        {
            var current = entities[i - 1];
            var currentGlobalPosition = current.hasGlobalTransform ? current.globalTransform.position : current.GetGlobalPositionVector2(gameContext);
            var currentPartHasHealthPoints = current.TryGetFirstGameEntity(gameContext, part => part.hasHealthPoints && !part.isInvulnerable, out var currentHealthPart);
            GameEntity currentBonusPickerPart = null;
            var currentPartCanPickBonuses = !current.isPassingThrough && current.TryGetFirstGameEntity(gameContext, part => part.isBonusPickable, out currentBonusPickerPart);
            var currentDamage = current.hasDamage ? (current.isPassingThrough && !current.isCollapses ? current.damage.value * Clock.deltaTime : current.damage.value) : 0f;
            var currentGrandOwnerId = current.hasGrandOwner ? current.grandOwner.id : current.id.value;
            var currentGrandParentId = current.GetGrandParent(gameContext).id.value;
            var currentIsTargetingParasite = current.isParasite && current.hasTarget;
            var currentGrandTargetId = current.hasTarget ? gameContext.GetEntityWithId(current.target.id).GetGrandParent(gameContext).id.value : 0;
            for (int j = i; j < count; j++)
            {
                var e = entities[j];
                var eGrandOwnerId = e.hasGrandOwner ? e.grandOwner.id : e.id.value;
                var eGrandParentId = e.GetGrandParent(gameContext).id.value;
                var eGlobalPosition = e.hasGlobalTransform ? e.globalTransform.position : e.GetGlobalPositionVector2(gameContext);
                //TODO: возможно, стоит убрать эту проверку
                if (eGrandOwnerId == currentGrandOwnerId) continue;
                if ((e.isIgnoringParentCollision || current.isIgnoringParentCollision) && currentGrandParentId == eGrandParentId) continue;
                var distance = eGlobalPosition - currentGlobalPosition;
                var closeDistance = e.circleCollider.radius + current.circleCollider.radius;
                var sqrDistance = distance.sqrMagnitude;
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
                        if (!current.isPassingThrough && !e.isPassingThrough &&
                            !((currentIsTargetingParasite && currentGrandTargetId == eGrandParentId) ||
                              (e.isParasite && e.hasTarget && gameContext.GetEntityWithId(e.target.id).GetGrandParent(gameContext).id.value == currentGrandParentId)))
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
                            if(ePart.healthPoints.value <= 0 && !ePart.hasKilledBy) ePart.AddKilledBy(currentGrandOwnerId);
                        }
                        if (e.hasDamage && currentPartHasHealthPoints)
                        {
                            var eDamage = e.isPassingThrough && !e.isCollapses ? e.damage.value * Clock.deltaTime : e.damage.value;
                            currentHealthPart.ReplaceHealthPoints(currentHealthPart.healthPoints.value - eDamage);
                            if (currentHealthPart.healthPoints.value <= 0 && !currentHealthPart.hasKilledBy) currentHealthPart.AddKilledBy(e.GetGrandOwnerId(gameContext));
                        }

                        if ((current.hasBonusAdder || current.hasActionBonus) && !current.hasBonusTarget && !e.isPassingThrough && e.TryGetFirstGameEntity(gameContext, part => part.isBonusPickable, out var eBonusPickerPart))
                        {
                            current.AddBonusTarget(eBonusPickerPart.id.value);
                        }
                        else if ((e.hasBonusAdder || e.hasActionBonus) && !e.hasBonusTarget && currentPartCanPickBonuses)
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
        var circleRadius = circle.circleCollider.radius;
        var circleGlobalPosition = circle.hasGlobalTransform ? circle.globalTransform.position : circle.GetGlobalPositionVector2(gameContext);
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        var dots = figure.globalPathCollider.dots;
        var axises = figure.globalNoncollinearAxises.vectors;

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
        var prevDepth = float.PositiveInfinity;
        penetration = new Vector2(0, 0);
        var axises1 = figure1.globalNoncollinearAxises.vectors;
        var dots1 = figure1.globalPathCollider.dots;
        var axises2 = figure2.globalNoncollinearAxises.vectors;
        var dots2 = figure2.globalPathCollider.dots;

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
                    penetrationVector = -depth * axis;
                    prevDepth = depth;
                }
            }
            else
            {
                depth = max2 - min1;
                if (depth < 0) return false;
                if (depth < prevDepth)
                {
                    penetrationVector = depth * axis;
                    prevDepth = depth;
                }
            }

            return true;
        }
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
}
