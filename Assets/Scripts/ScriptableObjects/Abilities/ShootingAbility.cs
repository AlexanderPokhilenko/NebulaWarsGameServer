using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShootingAbility", menuName = "Abilities/ShootingAbility", order = 51)]
public class ShootingAbility : ActiveAbility
{
    public BulletObject bullet;
    public bool usePositionVector;
    public Vector2 positionVector;
    public CannonsDischargeType discharge;

    protected override void AbilityAction(GameEntity executor, GameContext gameContext)
    {
        base.AbilityAction(executor, gameContext);

        var bulletOffset = usePositionVector ? positionVector :
            executor.hasCannon ? executor.cannon.position : positionVector;

        CannonShootingSystem.ShootBullet(executor, gameContext, bullet, bulletOffset);

        switch (discharge)
        {
            case CannonsDischargeType.None:
                break;
            case CannonsDischargeType.Main:
                if (executor.hasCannon)
                {
                    if (executor.hasCannonCooldown)
                    {
                        executor.ReplaceCannonCooldown(executor.cannonCooldown.value + bullet.lifetime);
                    }
                    else
                    {
                        executor.AddCannonCooldown(bullet.lifetime);
                    }
                }
                break;
            case CannonsDischargeType.All:
                foreach (var cannon in executor.GetAllChildrenGameEntities(gameContext, e => e.hasCannon))
                {
                    if (cannon.hasCannonCooldown)
                    {
                        cannon.ReplaceCannonCooldown(cannon.cannonCooldown.value + bullet.lifetime);
                    }
                    else
                    {
                        cannon.AddCannonCooldown(bullet.lifetime);
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum CannonsDischargeType
    {
        None,
        Main,
        All
    }
}
