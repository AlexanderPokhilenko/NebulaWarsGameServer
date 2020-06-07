using System.Collections.Generic;

public abstract class SpecialShooter
{
    private readonly List<GameEntity> cannonsBuffer = new List<GameEntity>();

    protected abstract IEnumerable<GameEntity> GetSpecialCannons(List<GameEntity> cannons);

    public IEnumerable<GameEntity> GetCannons(GameEntity entity, GameContext gameContext)
    {
        cannonsBuffer.Clear();

        FillBuffer(entity, gameContext);

        return GetSpecialCannons(cannonsBuffer);
    }

    private void FillBuffer(GameEntity entity, GameContext gameContext)
    {
        if (entity.hasCannon) cannonsBuffer.Add(entity);

        foreach (var child in gameContext.GetEntitiesWithParent(entity.id.value))
        {
            if (child.hasSpecialShooter)
            {
                cannonsBuffer.AddRange(child.specialShooter.value.GetCannons(child, gameContext));
            }
            else
            {
                FillBuffer(child, gameContext);
            }
        }
    }
}
