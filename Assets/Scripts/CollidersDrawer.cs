using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

//Внимание: сомнительное качество кода!
public sealed class CollidersDrawer : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private bool drawAxises = true;
    [SerializeField]
    private bool drawGuns = true;
    [SerializeField]
    private bool drawTargetingRadius = true;
    [SerializeField]
    private bool drawTargetLines = true;
    [SerializeField]
    private bool drawAuras = true;
#pragma warning restore 649
    private GameContext gameContext;
    private IGroup<GameEntity> collidableGroup;
    private IMatcher<GameEntity> matcher;

    public void Start()
    {
        gameContext = Contexts.sharedInstance.game;
        matcher = GameMatcher.AllOf(GameMatcher.Position).AnyOf(GameMatcher.CircleCollider, GameMatcher.PathCollider, GameMatcher.NoncollinearAxises, GameMatcher.Cannon, GameMatcher.TargetingParameters, GameMatcher.Target);
        collidableGroup = gameContext.GetGroup(matcher);
    }

    public void OnDrawGizmos()
    {
        if(collidableGroup == null) return;

        foreach (var e in collidableGroup)
        {
            var recursion = false;
            var firstParent = e;
            while (firstParent.hasParent)
            {
                firstParent = gameContext.GetEntityWithId(firstParent.parent.id);
                if (firstParent.id.value == e.id.value)
                {
                    Debug.LogError(nameof(CollidersDrawer) + " detected recursion!");
                    recursion = true;
                    break;
                }
            }
            if(recursion) continue;
            e.ToGlobal(gameContext, out var position, out var angle, out var layer, out var velocity, out var angularVelocity);
            var hasDirection = angle != 0;

            float sin = 0, cos = 1;
            if (hasDirection)
            {
                CoordinatesExtensions.GetSinCosFromDegrees(angle, out sin, out cos);
            }

            Gizmos.color = e.isCollidable ? Color.green : Color.gray;
            if (e.hasPathCollider)
            {
                var absoluteDots = new Vector2[e.pathCollider.dots.Length];
                for (var i = 0; i < e.pathCollider.dots.Length; i++)
                {
                    absoluteDots[i] = e.pathCollider.dots[i];
                    if (hasDirection)
                    {
                        absoluteDots[i].Rotate(sin, cos);
                    }
                    absoluteDots[i] += position;
                }

                var prevDot = absoluteDots[absoluteDots.Length - 1];
                foreach (var absoluteDot in absoluteDots)
                {
                    Gizmos.DrawLine(prevDot, absoluteDot);
                    prevDot = absoluteDot;
                }

                if (drawAxises && e.hasNoncollinearAxises)
                {
                    Gizmos.color = Color.yellow;
                    foreach (var axis in e.noncollinearAxises.vectors)
                    {
                        var absoluteVector = position;
                        if (hasDirection)
                        {
                            absoluteVector += axis.GetRotated(sin, cos);
                        }
                        else
                        {
                            absoluteVector += axis;
                        }
                        Gizmos.DrawLine(position, absoluteVector);
                    }
                }
            }
            else if(e.hasCircleCollider)
            {
                Gizmos.DrawWireSphere(position, e.circleCollider.radius);
            }

            if (drawGuns && e.hasCannon)
            {
                Gizmos.color = Color.red;

                var absoluteVector = position;
                if (hasDirection)
                {
                    absoluteVector += e.cannon.position.GetRotated(sin, cos);
                }
                else
                {
                    absoluteVector += e.cannon.position;
                }
                Gizmos.DrawLine(position, absoluteVector);
            }

            if (drawTargetingRadius && e.hasTargetingParameters)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(position, e.targetingParameters.radius);
            }

            if (drawTargetLines && e.hasTarget)
            {
                Gizmos.color = Color.cyan;
                var target = gameContext.GetEntityWithId(e.target.id);
                var targetPosition = target.GetGlobalPositionVector2(gameContext);
                Gizmos.DrawLine(position, targetPosition);
            }

            if (drawAuras && e.hasAura && e.hasCircleCollider)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(position, e.circleCollider.radius + e.aura.outerRadius);
            }
        }
    }
}
