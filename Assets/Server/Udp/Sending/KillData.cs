namespace Server.Udp.Sending
{
    public struct KillData
    {
        public int TargetPlayerId;
        public int KillerId;
        public ViewTypeId KillerType;
        public int VictimId;
        public ViewTypeId VictimType;
    }
}