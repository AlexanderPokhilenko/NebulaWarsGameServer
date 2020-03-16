using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class AddCollidersToPathSystem : ReactiveSystem<GameEntity>
{
    public AddCollidersToPathSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.PathCollider.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollidable && !entity.hasCircleCollider;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var dots = e.pathCollider.dots;

            var maxDist = Mathf.Sqrt(dots.Max(d => d.sqrMagnitude));
            e.AddCircleCollider(maxDist);

            if (dots.Length >= 2)
            {
                var sides = new Vector2[dots.Length];
                sides[0] = dots[0] - dots[dots.Length - 1];
                for (int i = 1; i < dots.Length; i++)
                {
                    sides[i] = dots[i] - dots[i - 1];
                }

                var prevSide = sides[sides.Length - 1];
                int sign = 0;
                bool isConcave = false;
                var axises = new HashSet<Vector2>();
                //    |///
                // ---+--- только I и IV квадранты
                //    |///
                foreach (var side in sides)
                {
                    var axis = new Vector2(-side.y, side.x);
                    //Рассматриваем оси с x > 0 и y > 0 при x = 0
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (axis.x < 0 || (axis.x == 0 && axis.y < 0))
                    {
                        axis = -axis;
                    }
                    axis.Normalize();
                    axises.Add(axis);
                    //Проверка на выпуклость: если знак векторного произведения сохраняется, то фигура выпуклая
                    if (!isConcave)
                    {
                        var crossProduct = prevSide.x * side.y - prevSide.y * side.x;
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (crossProduct != 0)
                        {
                            var newSign = Math.Sign(crossProduct);
                            if (sign == 0)
                            {
                                sign = newSign;
                            }
                            else
                            {
                                if (sign != newSign)
                                {
                                    isConcave = true;
                                }
                            }
                        }
                        prevSide = side;
                    }
                }

                e.AddNoncollinearAxises(axises.ToArray());
                e.isConcave = isConcave;
            }
        }
    }
}