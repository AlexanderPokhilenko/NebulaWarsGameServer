using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeroFormatter;

namespace NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    [ZeroFormattable]
    public struct RadiusesMessage : ITypedMessage
    {
        [Index(0)] public Dictionary<ushort, ushort> RadiusInfo { get; set; }

        [IgnoreFormat]
        public Dictionary<ushort, float> FloatRadiusInfo =>
            RadiusInfo.ToDictionary(pair => pair.Key, pair => Mathf.HalfToFloat(pair.Value));

        public RadiusesMessage(Dictionary<ushort, ushort> radiusInfo)
        {
            RadiusInfo = radiusInfo;
        }

        public MessageType GetMessageType() => MessageType.Radiuses;
    }
}