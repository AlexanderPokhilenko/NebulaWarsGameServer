using System.Collections.Generic;
using Entitas;
using Plugins.submodules.SharedCode.Logger;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    /// <summary>
    /// Отвечает за отправку сообщения о смертях, которые произошли за этот кадр.
    /// </summary>
    public class NetworkKillsSenderSystem : ReactiveSystem<ServerGameEntity>
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly PlayerDeathHandler playerDeathHandler;
        private readonly IGroup<ServerGameEntity> aliveAvatars;
        private readonly IGroup<ServerGameEntity> alivePlayers;
        private readonly KillMessagesFactory killMessagesFactory;
        private readonly ILog log = LogManager.CreateLogger(typeof(NetworkKillsSenderSystem));

        public NetworkKillsSenderSystem(Contexts contexts, int matchId, 
            PlayerDeathHandler playerDeathHandler,  UdpSendUtils udpSendUtils, Killers killers)
            : base(contexts.serverGame)
        {
            this.matchId = matchId;
            this.playerDeathHandler = playerDeathHandler;
            this.udpSendUtils = udpSendUtils;

            killMessagesFactory = new KillMessagesFactory(killers);
            aliveAvatars = contexts.serverGame.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player)
                .NoneOf(ServerGameMatcher.KilledBy));
            
            alivePlayers = contexts.serverGame.GetGroup(ServerGameMatcher.AllOf(ServerGameMatcher.Player)
                .NoneOf(ServerGameMatcher.KilledBy, ServerGameMatcher.Bot));
            
        }

        protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
        {
            return context.CreateCollector(ServerGameMatcher.KilledBy.Added());
        }

        protected override bool Filter(ServerGameEntity entity)
        {
            return entity.hasKilledBy;
        }

        protected override void Execute(List<ServerGameEntity> killedEntities)
        {
            SendDeathNoticeToPlayers(killedEntities);
            SendDeathNoticeToMatchmaker(killedEntities);
        }

        private void SendDeathNoticeToPlayers(List<ServerGameEntity> killedEntities)
        {
            List<KillModel> list = killMessagesFactory.Create(killedEntities);
            foreach (var alivePlayer in alivePlayers)
            {
                foreach (var model in list)
                {
                    udpSendUtils.SendKill(matchId, alivePlayer.player.tmpPlayerId, model);
                }
            }
        }
        
        private void SendDeathNoticeToMatchmaker(List<ServerGameEntity> killedEntities)
        {
            int aliveAvatarsCount = aliveAvatars.count;
            for (int index = 0; index < killedEntities.Count; index++)
            {
                ServerGameEntity killedEntity = killedEntities[index];
                if (!killedEntity.isBot)
                {
                    ushort temporaryId = killedEntity.player.tmpPlayerId;
                    int accountId = killedEntity.account.accountId;
                    int placeInBattle = aliveAvatarsCount + index + 1;
                    PlayerDeathData playerDeathData = new PlayerDeathData
                    {
                        PlayerAccountId = accountId,
                        PlaceInBattle = placeInBattle,
                        MatchId = matchId
                    };

                    playerDeathHandler.PlayerDeath(playerDeathData, temporaryId, true);
                }
            }
        }
    }
}
