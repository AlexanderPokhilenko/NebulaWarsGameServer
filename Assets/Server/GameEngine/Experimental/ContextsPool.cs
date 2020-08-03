using System.Collections.Concurrent;

namespace Server.GameEngine.Experimental
{
    public static class ContextsPool
    {
        private static readonly ConcurrentBag<Contexts> Pool = new ConcurrentBag<Contexts>();

        public static Contexts GetContexts() => Pool.TryTake(out var pooledContexts) ? pooledContexts : new Contexts();

        public static void RetrieveContexts(Contexts contexts) => Pool.Add(contexts);
    }
}