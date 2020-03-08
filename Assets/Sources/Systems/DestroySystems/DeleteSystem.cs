using Entitas;
using log4net;

public sealed class DeleteSystem : IExecuteSystem
{
    private IGroup<GameEntity> deletingGroup;
    private static readonly ILog Log = LogManager.GetLogger(typeof(DeleteSystem));
    
    public DeleteSystem(Contexts contexts)
    {
        deletingGroup = contexts.game.GetGroup(GameMatcher.Destroyed);
    }

    public void Execute()
    {
        //TODO: посмотреть, можно ли как-то удалять без использования массива
        foreach (var e in deletingGroup.GetEntities())
        {
            if(e.hasKilledBy) Log.Info($"Объект с id = {e.id.value} был уничтожен объектом с id = {e.killedBy.id}.");
            e.Destroy();
        }
    }
}
