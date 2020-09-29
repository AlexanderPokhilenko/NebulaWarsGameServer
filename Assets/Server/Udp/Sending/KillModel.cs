namespace Server.Udp.Sending
{
    public struct KillModel
    {
        public int killerId;
        public ViewTypeEnum killerType;
        public int victimId;
        public ViewTypeEnum victimType;
    }
}