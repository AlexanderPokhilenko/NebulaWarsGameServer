﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections.Generic;
using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    [ZeroFormattable]
    public class PositionsMessage : ITypedMessage
    {
        [Index(0)] public virtual Dictionary<int, ViewTransform> EntitiesInfo { get; set; }
        //TODO: перенести в UDP с подтверждением
        [Index(1)] public virtual int PlayerEntityId { get; set; }

        [Index(2)] public virtual Dictionary<int, float> RadiusInfo { get; set; }

        public PositionsMessage()
        {
            //EntitiesInfo = new Dictionary<int, ViewTransform>();
        }

        public MessageType GetMessageType() => MessageType.Positions;
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

        public override string ToString()
        {
            return $"({X}; {Y})";
        }

#if UNITY_5_3_OR_NEWER
        public static implicit operator UnityEngine.Vector2(Vector2 vector) => new UnityEngine.Vector2(vector.X, vector.Y);
        public static implicit operator Vector2(UnityEngine.Vector2 vector) => new Vector2(vector.x, vector.y);
#endif
    }
    
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
    
    [ZeroFormattable]
    public struct DebugMessage:ITypedMessage
    {
        [Index(0)] public uint MessageNumberThatConfirms;

        public DebugMessage(uint messageNumberThatConfirms)
        {
            MessageNumberThatConfirms = messageNumberThatConfirms;
        }
        
        public MessageType GetMessageType() => MessageType.Debug;
    }
    
    [ZeroFormattable]
    public class TestMessageClass1
    {
        [Index(0)] public virtual int SomeNumber { get; set; }

        public TestMessageClass1()
        {
            
        }
    }
    
    [ZeroFormattable]
    public struct TestMessageStruct1
    {
        [Index(0)] public int SomeNumber;

        public TestMessageStruct1(int someNumber)
        {
            SomeNumber = someNumber;
        }
    }
    
}