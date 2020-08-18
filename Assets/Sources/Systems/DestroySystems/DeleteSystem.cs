using System.Collections.Generic;
using Code.Common;
using Entitas;
using Libraries.Logger;

public sealed class DeleteSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly IGroup<GameEntity> deletingGroup;
    private static readonly ILog Log = LogManager.CreateLogger(typeof(DeleteSystem));
    
    public DeleteSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        deletingGroup = contexts.game.GetGroup(GameMatcher.Destroyed);
    }

    public void Execute()
    {
        foreach (var e in deletingGroup.GetEntities(buffer))
        {
            e.Destroy();
        }
    }
}
