using System.Collections.Generic;
using Entitas;
using log4net;

public sealed class DeleteSystem : IExecuteSystem
{
    private readonly List<GameEntity> buffer;
    private readonly IGroup<GameEntity> deletingGroup;
    private static readonly ILog Log = LogManager.GetLogger(typeof(DeleteSystem));
    
    public DeleteSystem(Contexts contexts)
    {
        buffer = new List<GameEntity>();
        deletingGroup = contexts.game.GetGroup(GameMatcher.Destroyed);
    }

    public void Execute()
    {
        foreach (var e in deletingGroup.GetEntities(buffer))
        {
            if(e.hasKilledBy) Log.Info($"Объект с id = {e.id.value} был уничтожен объектом с id = {e.killedBy.id}.");
            e.Destroy();
        }
    }
}
