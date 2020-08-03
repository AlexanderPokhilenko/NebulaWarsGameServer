using System.Collections.Generic;
using Entitas;
using Server.GameEngine;
using Server.GameEngine.Experimental;

public sealed class UpdatePositionChunksSystem : IExecuteSystem
{
    private readonly PositionChunks chunks;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> transformGroup;
    private readonly List<GameEntity> buffer;
    private const int PredictedCapacity = 512;

    public UpdatePositionChunksSystem(Contexts contexts, PositionChunks positionChunks)
    {
        chunks = positionChunks;
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.Collidable, GameMatcher.CircleCollider, GameMatcher.Position);
        transformGroup = gameContext.GetGroup(matcher);
        buffer = new List<GameEntity>(PredictedCapacity);
    }

    public void Execute()
    {
        chunks.Fill(transformGroup.GetEntities(buffer), gameContext);
    }
}