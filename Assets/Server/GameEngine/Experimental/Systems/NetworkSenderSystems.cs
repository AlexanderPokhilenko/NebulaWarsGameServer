using Server.GameEngine.Systems;
using Server.Udp.Sending;

internal class NetworkSenderSystems : Feature
{
    public NetworkSenderSystems(Contexts contexts, int matchId, UdpSendUtils udpSendUtils) : base("Network Sender Systems")
    {
        Add(new PositionsSenderSystem(contexts, matchId, udpSendUtils));
        Add(new RadiusesUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new FinalRadiusesSystem(contexts, matchId, udpSendUtils));
        Add(new HealthUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new MaxHpUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new CooldownInfoUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new CooldownUpdaterSystem(contexts, matchId, udpSendUtils));
        Add(new ShieldPointsUpdaterSystem(contexts, matchId, udpSendUtils));
    }
}