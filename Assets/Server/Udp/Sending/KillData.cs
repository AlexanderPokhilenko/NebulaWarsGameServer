namespace Server.Udp.Sending
{
    public struct KillData
    {
        public ushort TargetPlayerTmpId;
        public int KillerId;
        public ViewTypeId KillerType;
        public int VictimId;
        public ViewTypeId VictimType;
    }
}