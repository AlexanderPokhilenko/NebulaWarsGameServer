using JetBrains.Annotations;
using Plugins.submodules.SharedCode.Logger;

namespace Server.GameEngine.Experimental.Systems
{
    public class Killers
    {
        private readonly Contexts contexts;
        private readonly ILog log = LogManager.CreateLogger(typeof(Killers));

        public Killers(Contexts contexts)
        {
            this.contexts = contexts;
        }

        [CanBeNull]
        public KillerInfo GetKillerInfo(int accountId)
        {
            ServerGameEntity entity = contexts.serverGame.GetEntityWithAccount(accountId);
            if (entity == null)
            {
                log.Error("Не найдена сущность убийцы.");
                return null;
            }
            
            if (!entity.hasViewType)
            {
                log.Error("У убийцы нет внешнего вида");
                return null;
            }

            KillerInfo result = new KillerInfo(entity.player.tmpPlayerId, entity.viewType.value);
            log.Debug($"killer {result.type} {result.playerId}");
            return result;
        }
    }
}