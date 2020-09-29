using System;
using System.Collections.Generic;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.Logger;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class KillMessagesFactory
    {
        private readonly Killers killers;
        private readonly ILog log = LogManager.CreateLogger(typeof(KillMessagesFactory));

        public KillMessagesFactory(Killers killers)
        {
            this.killers = killers;
        }
        
        public List<KillModel> Create(List<ServerGameEntity> killedEntities)
        {
            List<KillModel> result = new List<KillModel>();
            try
            {
                for (var killedEntityIndex = 0; killedEntityIndex < killedEntities.Count; killedEntityIndex++)
                {
                    ServerGameEntity killedEntity = killedEntities[killedEntityIndex];
                    KillerInfo killerInfo = killers.GetKillerInfo(killedEntity.killedBy.id);
                    if (killerInfo == null)
                    {
                        log.Error("Нет информации про убийцу.");
                        continue;
                    }

                    KillModel killModel = new KillModel
                    {
                        killerId = killerInfo.playerId,
                        killerType = killerInfo.type,
                        victimType = killedEntity.viewType.value,
                        victimId = killedEntity.account.accountId
                    };
                    result.Add(killModel);
                }
            }
            catch (Exception e)
            {
                log.Error(e.FullMessage());
            }
            
            return result;
        }
    }
}