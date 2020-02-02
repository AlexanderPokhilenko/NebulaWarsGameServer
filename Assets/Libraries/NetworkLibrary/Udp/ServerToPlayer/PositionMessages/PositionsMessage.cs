using System.Collections.Generic;
using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    [ZeroFormattable]
    public class PositionsMessage
    {
        [Index(0)] public virtual Dictionary<int, Transform> EntitiesInfo { get; set; }

        public PositionsMessage()
        {
            EntitiesInfo = new Dictionary<int, Transform>();
        }
    }

    [ZeroFormattable]
    public struct Vector2
    {
        [Index(0)] public float X;
        [Index(1)] public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator UnityEngine.Vector2(Vector2 vector) => new UnityEngine.Vector2(vector.X, vector.Y);
        public static implicit operator Vector2(UnityEngine.Vector2 vector) => new Vector2(vector.x, vector.y);
    }

    [ZeroFormattable]
    public struct Transform
    {
        [Index(0)] public float x;
        [Index(1)] public float y;
        [Index(2)] public float angle;

        public Transform(float x, float y, float angle)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
        }

        public Transform(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.angle = 0f;
        }

        public Transform(Vector2 position)
        {
            this.x = position.X;
            this.y = position.Y;
            this.angle = 0f;
        }

        public static Transform GetTransform(GameEntity entity)
        {
            var position = entity.position.value;
            var direction = entity.direction.angle;
            return new Transform(position.x, position.y, direction);
        }

        public Vector2 GetPosition() => new Vector2(x, y);
    }
}