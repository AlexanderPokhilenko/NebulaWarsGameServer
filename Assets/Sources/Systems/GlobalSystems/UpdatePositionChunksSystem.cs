using System.Collections.Generic;
using Entitas;
using Server.GameEngine;

public sealed class UpdatePositionChunksSystem : IExecuteSystem
{
    private readonly PositionChunks chunks;
    private readonly GameContext gameContext;
    private readonly IGroup<GameEntity> transformGroup;
    private readonly List<GameEntity> buffer;
    private readonly int PredictedCapacity = 512;

    public UpdatePositionChunksSystem(Contexts contexts, PositionChunks positionChunks)
    {
        chunks = positionChunks;
        gameContext = contexts.game;
        var matcher = GameMatcher.AllOf(GameMatcher.CircleCollider, GameMatcher.GlobalTransform);
        transformGroup = gameContext.GetGroup(matcher);
        buffer = new List<GameEntity>(PredictedCapacity);
    }

    public void Execute()
    {
        chunks.Fill(transformGroup.GetEntities(buffer));
    }
}