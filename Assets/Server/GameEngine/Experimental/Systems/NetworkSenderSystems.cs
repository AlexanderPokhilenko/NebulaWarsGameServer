using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    internal class NetworkSenderSystems : Feature
    {
        public NetworkSenderSystems(Contexts contexts, int matchId, UdpSendUtils udpSendUtils, PlayersViewAreas viewAreas) : base("Network Sender Systems")
        {
            Add(new FrameRateSenderSystem(contexts, matchId, udpSendUtils));

            Add(new HidesSenderSystem(contexts, matchId, udpSendUtils, viewAreas));
            Add(new ChangingPositionsSenderSystem(contexts, matchId, udpSendUtils, viewAreas));
            Add(new UnhiddenStoppedSenderSystem(contexts, matchId, udpSendUtils, viewAreas));

            Add(new RadiusesUpdaterSystem(contexts, matchId, udpSendUtils, viewAreas));
            Add(new FinalRadiusesSystem(contexts, matchId, udpSendUtils, viewAreas));

            Add(new ParentsSenderSystem(contexts, matchId, udpSendUtils));
            Add(new DetachesSenderSystem(contexts, matchId, udpSendUtils));

            Add(new HealthUpdaterSystem(contexts, matchId, udpSendUtils, viewAreas));
            Add(new MaxHpUpdaterSystem(contexts, matchId, udpSendUtils, viewAreas));

            Add(new TeamsUpdaterSystem(contexts, matchId, udpSendUtils));

            Add(new CooldownInfoUpdaterSystem(contexts, matchId, udpSendUtils));
            Add(new CooldownUpdaterSystem(contexts, matchId, udpSendUtils));

            Add(new DestroysSenderSystem(contexts, matchId, udpSendUtils));
        }
    }
}