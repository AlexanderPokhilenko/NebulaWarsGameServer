namespace Server.Udp.Sending
{
    public struct KillData
    {
        public ushort TargetPlayerTmpId;
        public int KillerId;
        public ViewTypeEnum KillerType;
        public int VictimId;
        public ViewTypeEnum VictimType;
    }
}