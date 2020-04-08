using UnityEngine;

[CreateAssetMenu(fileName = "NewShootingAbility", menuName = "Abilities/ShootingAbility", order = 51)]
public class ShootingAbility : ActiveAbility
{
    public BulletObject bullet;
    public bool usePositionVector;
    public Vector2 positionVector;

    protected override void AbilityAction(GameEntity executor, GameContext gameContext)
    {
        base.AbilityAction(executor, gameContext);

        var bulletOffset = usePositionVector ? positionVector :
            executor.hasCannon ? executor.cannon.position : positionVector;

        CannonShootingSystem.ShootBullet(executor, gameContext, bullet, bulletOffset);
    }
}
