public abstract class ActionBonusObject : BaseObject
{
    public override GameEntity CreateEntity(GameContext context)
    {
        var entity = base.CreateEntity(context);
        entity.AddActionBonus(Action);

        return entity;
    }

    protected abstract void Action(GameEntity entity);
}
