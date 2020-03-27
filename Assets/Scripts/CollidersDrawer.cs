#if UNITY_EDITOR
using System;
using Entitas;
using System.Collections.Generic;
using log4net;
using UnityEditor;
using UnityEngine;

//Внимание: сомнительное качество кода!
public sealed class CollidersDrawer : MonoBehaviour
{

    [SerializeField] private bool drawAxises = true;
    [SerializeField] private bool drawGuns = true;
    [SerializeField] private bool drawTargetingRadius = true;
    [SerializeField] private bool drawTargetLines = true;
    [SerializeField] private bool drawAuras = true;
    [SerializeField] private bool drawTargetMovingPoints = true;
    [SerializeField] private bool changeContext = true;
    [SerializeField, Min(0)] private int contextIndex = 0;

    public static List<Contexts> contextsList = new List<Contexts>();
    private GameContext currentGameContext;
    private IGroup<GameEntity> collidableGroup;
    private IMatcher<GameEntity> matcher = GameMatcher.AllOf(GameMatcher.Position).AnyOf(GameMatcher.CircleCollider, GameMatcher.PathCollider, GameMatcher.NoncollinearAxises, GameMatcher.Cannon, GameMatcher.TargetingParameters, GameMatcher.Target);

    private static readonly ILog Log = LogManager.GetLogger(typeof(CollidersDrawer));

    public bool ChangeContext(GameContext gameContext)
    {
        currentGameContext = gameContext;
        if (currentGameContext == null) return false;
        collidableGroup = currentGameContext.GetGroup(matcher);
        return true;
    }

    public void OnDrawGizmos()
    {
        try
        {
            Draw();
        }
        catch
        {
            // ignored
        }
    }

    public void OnDestroy()
    {
        contextsList.Clear();
    }

    private void Draw()
    {
         if (changeContext && contextIndex >= 0 && contextIndex < contextsList.Count)
         {
             if(!ChangeContext(contextsList[contextIndex].game)) return;
             changeContext = false;
         }

         if(currentGameContext == null || collidableGroup == null) return;

         foreach (var e in collidableGroup)
         {
             var recursion = false;
             var firstParent = e;
             while (firstParent.hasParent)
             {
                 firstParent = currentGameContext.GetEntityWithId(firstParent.parent.id);
                 if (firstParent.id.value == e.id.value)
                 {
                     Log.Error(nameof(CollidersDrawer) + " detected recursion!");
                     recursion = true;
                     break;
                 }
             }
             if(recursion) continue;
             e.ToGlobal(currentGameContext, out var position, out var angle, out var layer, out var velocity, out var angularVelocity);
             var hasDirection = Math.Abs(angle) > 0.001;

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
                 var target = currentGameContext.GetEntityWithId(e.target.id);
                 var targetPosition = target.GetGlobalPositionVector2(currentGameContext);
                 Gizmos.DrawLine(position, targetPosition);
             }

             if (drawAuras && e.hasAura && e.hasCircleCollider)
             {
                 Gizmos.color = Color.blue;
                 Gizmos.DrawWireSphere(position, e.circleCollider.radius + e.aura.outerRadius);
             }

             if (drawTargetMovingPoints && e.hasTargetMovingPoint)
             {
                 Gizmos.color = Color.black;
                 var movingPosition = e.targetMovingPoint.position;
                 Gizmos.DrawLine(position, movingPosition);
             }
         }
    }
}
#endif