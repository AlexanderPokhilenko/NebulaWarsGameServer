using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    [ZeroFormattable]
    public struct ViewTransform
    {
        [Index(0)] public float x;
        [Index(1)] public float y;
        [Index(2)] public float angle;
        [Index(3)] public ViewTypeId typeId;

        public ViewTransform(float x, float y, float angle, ViewTypeId typeId)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.typeId = typeId;
        }

        public ViewTransform(float x, float y, ViewTypeId typeId)
        {
            this.x = x;
            this.y = y;
            this.angle = 0f;
            this.typeId = typeId;
        }

        public ViewTransform(Vector2 position, ViewTypeId typeId)
        {
            this.x = position.X;
            this.y = position.Y;
            this.angle = 0f;
            this.typeId = typeId;
        }

        public ViewTransform(Vector2 position, float angle, ViewTypeId typeId)
        {
            this.x = position.X;
            this.y = position.Y;
            this.angle = angle;
            this.typeId = typeId;
        }

        public Vector2 GetPosition() => new Vector2(x, y);
    }
}