using Server.GameEngine;
using Server.GameEngine.Systems;
using Server.Udp.Sending;

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

        Add(new HealthUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new MaxHpUpdaterSystem(contexts, matchId, udpSendUtils));

        Add(new CooldownInfoUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new CooldownUpdaterSystem(contexts, matchId, udpSendUtils));

        Add(new ShieldPointsUpdaterSystem(contexts, matchId, udpSendUtils));

        Add(new DestroysSenderSystem(contexts, matchId, udpSendUtils));
    }
}