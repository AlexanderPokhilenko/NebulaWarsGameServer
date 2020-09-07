using System.Linq;
using NetworkLibrary.NetworkLibrary.Http;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.Physics;
using Plugins.submodules.SharedCode.Systems.Spawn;
using Server.GameEngine.Chronometers;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using SharedSimulationCode;

namespace Server.GameEngine.MatchLifecycle
{
    public class MatchFactory
    {
        private readonly MatchRemover matchRemover;
        private readonly UdpSendUtils udpSendUtils;
        private readonly IPrefabStorage prefabsStorage;
        private readonly MessageIdFactory messageIdFactory;
        private readonly MatchmakerNotifier matchmakerNotifier;
        private readonly IpAddressesStorage ipAddressesStorage;
        private readonly ITickStartTimeStorage tickStartTimeStorage;
        private readonly ITickDeltaTimeStorage tickDeltaTimeStorage;
        private readonly MessagesPackIdFactory messagesPackIdFactory;
        private readonly WarshipsCharacteristicsStorage warshipsCharacteristicsStorage;

        public MatchFactory(MatchRemover matchRemover, UdpSendUtils udpSendUtils,
            MatchmakerNotifier matchmakerNotifier, IpAddressesStorage ipAddressesStorage,
            MessageIdFactory messageIdFactory, MessagesPackIdFactory messagesPackIdFactory,
            ITickDeltaTimeStorage tickDeltaTimeStorage, ITickStartTimeStorage tickStartTimeStorage,
            IPrefabStorage prefabsStorage, WarshipsCharacteristicsStorage warshipsCharacteristicsStorage )
        {
            this.matchRemover = matchRemover;
            this.udpSendUtils = udpSendUtils;
            this.messageIdFactory = messageIdFactory;
            this.messagesPackIdFactory = messagesPackIdFactory;
            this.tickDeltaTimeStorage = tickDeltaTimeStorage;
            this.tickStartTimeStorage = tickStartTimeStorage;
            this.prefabsStorage = prefabsStorage;
            this.warshipsCharacteristicsStorage = warshipsCharacteristicsStorage;
            this.matchmakerNotifier = matchmakerNotifier;
            this.ipAddressesStorage = ipAddressesStorage;
        }
        
        public ServerMatchSimulation Create(BattleRoyaleMatchModel matchModel)
        {
            ipAddressesStorage.AddMatch(matchModel);

            foreach (ushort playerId in matchModel.GameUnits.Players.Select(player=>player.TemporaryId))
            {
                messageIdFactory.AddPlayer(matchModel.MatchId, playerId);
                messagesPackIdFactory.AddPlayer(matchModel.MatchId, playerId);
            }
            
            ServerMatchSimulation serverMatch = new ServerMatchSimulation(
                matchModel.MatchId,
                matchModel,
                udpSendUtils,
                ipAddressesStorage,
                matchRemover,
                matchmakerNotifier,
                tickDeltaTimeStorage,
                tickStartTimeStorage,
                prefabsStorage,
                warshipsCharacteristicsStorage);
            
            serverMatch.Initialize();
            return serverMatch;
        }
    }
}