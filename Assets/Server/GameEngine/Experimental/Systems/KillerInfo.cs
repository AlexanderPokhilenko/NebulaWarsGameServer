namespace Server.GameEngine.Experimental.Systems
{
    public class KillerInfo
    {
        public readonly int playerId;
        public readonly ViewTypeEnum type;

        public KillerInfo(int playerId, ViewTypeEnum type)
        {
            this.playerId = playerId;
            this.type = type;
        }
    }
}