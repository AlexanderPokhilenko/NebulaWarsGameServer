using System.Linq;
using Entitas;

public static class ContextsCollectorsExtensions
{
    public static ICollector<TEntity> AnyChange<TContext, TEntity>(this TContext context, params IMatcher<TEntity>[] matchers)
        where TContext : class, IContext<TEntity>
        where TEntity : class, IEntity
    {
        return new Collector<TEntity>(
            matchers.Select(m => context.GetGroup(m)).ToArray(),
            matchers.Select(_ => GroupEvent.AddedOrRemoved).ToArray());
    }
}