#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;
    using global::ZeroFormatter.Comparers;

    public static partial class ZeroFormatterInitializer
    {
        static bool registered = false;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            if(registered) return;
            registered = true;
            // Enums
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::ViewTypeId>.Register(new ZeroFormatter.DynamicObjectSegments.ViewTypeIdFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::ViewTypeId>.Register(new ZeroFormatter.DynamicObjectSegments.ViewTypeIdEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::ViewTypeId?>.Register(new ZeroFormatter.DynamicObjectSegments.NullableViewTypeIdFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::ViewTypeId?>.Register(new NullableEqualityComparer<global::ViewTypeId>());
            
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnumFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnumEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum?>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.NullableGameRoomValidationResultEnumFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum?>.Register(new NullableEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>());
            
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.MatchType>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.MatchTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.MatchType>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.MatchTypeEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.MatchType?>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.NullableMatchTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.MatchType?>.Register(new NullableEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.MatchType>());
            
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessageTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Udp.MessageType>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessageTypeEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType?>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.NullableMessageTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Udp.MessageType?>.Register(new NullableEqualityComparer<global::NetworkLibrary.NetworkLibrary.Udp.MessageType>());
            
            // Objects
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.WarshipCopyFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.AccountInfo>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.AccountInfoFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatchFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.BotInfo>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.BotInfoFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatchFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchDataFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameMatcherResponse>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameMatcherResponseFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnit>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameUnitFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessage>.Register(new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1Formatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessagesContainer>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessagesContainerFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            // Structs
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfoFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransformFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2Formatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1Formatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessageWrapperFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper>(structFormatter));
            }
            // Unions
            // Generics
            ZeroFormatter.Formatters.Formatter.RegisterDictionary<ZeroFormatter.Formatters.DefaultResolver, int, float>();
            ZeroFormatter.Formatters.Formatter.RegisterDictionary<ZeroFormatter.Formatters.DefaultResolver, int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, byte[]>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, string>();
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.BotInfo, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo>>();
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>>();
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>>();
        }
    }
}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class WarshipCopyFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (4 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 0, value.PrefabName);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.CombatPowerLevel);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 2, value.CombatPowerValue);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.Rating);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 4, value.Id);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 4);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new WarshipCopyObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class WarshipCopyObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4, 4, 4, 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _PrefabName;

        // 0
        public override string PrefabName
        {
            get
            {
                return _PrefabName.Value;
            }
            set
            {
                _PrefabName.Value = value;
            }
        }

        // 1
        public override int CombatPowerLevel
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override int CombatPowerValue
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 3
        public override int Rating
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 4
        public override int Id
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }


        public WarshipCopyObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 4, __elementSizes);

            _PrefabName = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (4 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 0, ref _PrefabName);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 2, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 4, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 4);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class AccountInfoFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.AccountInfo>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.AccountInfo value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (6 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 0, value.Username);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.AccountRating);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 2, value.RegularCurrency);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.PremiumCurrency);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 4, value.PointsForBigChest);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 5, value.PointsForSmallChest);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>>(ref bytes, startOffset, offset, 6, value.Warships);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 6);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.AccountInfo Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new AccountInfoObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class AccountInfoObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.AccountInfo, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4, 4, 4, 4, 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _Username;
        CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>> _Warships;

        // 0
        public override string Username
        {
            get
            {
                return _Username.Value;
            }
            set
            {
                _Username.Value = value;
            }
        }

        // 1
        public override int AccountRating
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override int RegularCurrency
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 3
        public override int PremiumCurrency
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 4
        public override int PointsForBigChest
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 5
        public override int PointsForSmallChest
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 6
        public override global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy> Warships
        {
            get
            {
                return _Warships.Value;
            }
            set
            {
                _Warships.Value = value;
            }
        }


        public AccountInfoObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 6, __elementSizes);

            _Username = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
            _Warships = new CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 6, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (6 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 0, ref _Username);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 2, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 4, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 5, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.WarshipCopy>>(ref targetBytes, startOffset, offset, 6, ref _Warships);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 6);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class PlayerInfoForMatchFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (6 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 0, value.TemporaryId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 1, value.IsBot);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 2, value.PrefabName);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.WarshipCombatPowerLevel);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 4, value.ServiceId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 5, value.AccountId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 6, value.SomeDichMegaData);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 6);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new PlayerInfoForMatchObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class PlayerInfoForMatchObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 1, 0, 4, 0, 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _PrefabName;
        CacheSegment<TTypeResolver, string> _ServiceId;
        CacheSegment<TTypeResolver, string> _SomeDichMegaData;

        // 0
        public override int TemporaryId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override bool IsBot
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override string PrefabName
        {
            get
            {
                return _PrefabName.Value;
            }
            set
            {
                _PrefabName.Value = value;
            }
        }

        // 3
        public override int WarshipCombatPowerLevel
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 4
        public override string ServiceId
        {
            get
            {
                return _ServiceId.Value;
            }
            set
            {
                _ServiceId.Value = value;
            }
        }

        // 5
        public override int AccountId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 6
        public override string SomeDichMegaData
        {
            get
            {
                return _SomeDichMegaData.Value;
            }
            set
            {
                _SomeDichMegaData.Value = value;
            }
        }


        public PlayerInfoForMatchObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 6, __elementSizes);

            _PrefabName = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
            _ServiceId = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 4, __binaryLastIndex, __tracker));
            _SomeDichMegaData = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 6, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (6 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 2, ref _PrefabName);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 4, ref _ServiceId);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 5, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 6, ref _SomeDichMegaData);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 6);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class BotInfoFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.BotInfo>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.BotInfo value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (4 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 0, value.TemporaryId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 1, value.IsBot);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 2, value.PrefabName);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.WarshipCombatPowerLevel);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 4, value.BotName);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 4);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.BotInfo Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new BotInfoObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class BotInfoObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.BotInfo, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 1, 0, 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _PrefabName;
        CacheSegment<TTypeResolver, string> _BotName;

        // 0
        public override int TemporaryId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override bool IsBot
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override string PrefabName
        {
            get
            {
                return _PrefabName.Value;
            }
            set
            {
                _PrefabName.Value = value;
            }
        }

        // 3
        public override int WarshipCombatPowerLevel
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 4
        public override string BotName
        {
            get
            {
                return _BotName.Value;
            }
            set
            {
                _BotName.Value = value;
            }
        }


        public BotInfoObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 4, __elementSizes);

            _PrefabName = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
            _BotName = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 4, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (4 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 2, ref _PrefabName);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 4, ref _BotName);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 4);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameUnitsForMatchFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (1 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>>(ref bytes, startOffset, offset, 0, value.Players);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo>>(ref bytes, startOffset, offset, 1, value.Bots);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 1);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameUnitsForMatchObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameUnitsForMatchObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>> _Players;
        CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo>> _Bots;

        // 0
        public override global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch> Players
        {
            get
            {
                return _Players.Value;
            }
            set
            {
                _Players.Value = value;
            }
        }

        // 1
        public override global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo> Bots
        {
            get
            {
                return _Bots.Value;
            }
            set
            {
                _Bots.Value = value;
            }
        }


        public GameUnitsForMatchObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 1, __elementSizes);

            _Players = new CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
            _Bots = new CacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 1, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (1 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForMatch>>(ref targetBytes, startOffset, offset, 0, ref _Players);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.List<global::NetworkLibrary.NetworkLibrary.Http.BotInfo>>(ref targetBytes, startOffset, offset, 1, ref _Bots);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 1);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class BattleRoyaleMatchDataFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (3 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 0, value.GameServerIp);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.GameServerPort);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 2, value.MatchId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch>(ref bytes, startOffset, offset, 3, value.GameUnitsForMatch);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 3);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new BattleRoyaleMatchDataObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class BattleRoyaleMatchDataObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4, 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _GameServerIp;
        global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch _GameUnitsForMatch;

        // 0
        public override string GameServerIp
        {
            get
            {
                return _GameServerIp.Value;
            }
            set
            {
                _GameServerIp.Value = value;
            }
        }

        // 1
        public override int GameServerPort
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override int MatchId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 3
        public override global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch GameUnitsForMatch
        {
            get
            {
                return _GameUnitsForMatch;
            }
            set
            {
                __tracker.Dirty();
                _GameUnitsForMatch = value;
            }
        }


        public BattleRoyaleMatchDataObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 3, __elementSizes);

            _GameServerIp = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
            _GameUnitsForMatch = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch>(originalBytes, 3, __binaryLastIndex, __tracker);
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (3 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 0, ref _GameServerIp);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 2, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnitsForMatch>(ref targetBytes, startOffset, offset, 3, _GameUnitsForMatch);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 3);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameMatcherResponseFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameMatcherResponse>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameMatcherResponse value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (5 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 0, value.PlayerHasJustBeenRegistered);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 1, value.PlayerInQueue);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 2, value.PlayerInBattle);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData>(ref bytes, startOffset, offset, 3, value.BattleRoyaleMatchData);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 4, value.NumberOfPlayersInQueue);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 5, value.NumberOfPlayersInBattles);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 5);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameMatcherResponse Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameMatcherResponseObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameMatcherResponseObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameMatcherResponse, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 1, 1, 1, 0, 4, 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData _BattleRoyaleMatchData;

        // 0
        public override bool PlayerHasJustBeenRegistered
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override bool PlayerInQueue
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override bool PlayerInBattle
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 2, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 3
        public override global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData BattleRoyaleMatchData
        {
            get
            {
                return _BattleRoyaleMatchData;
            }
            set
            {
                __tracker.Dirty();
                _BattleRoyaleMatchData = value;
            }
        }

        // 4
        public override int NumberOfPlayersInQueue
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 4, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 5
        public override int NumberOfPlayersInBattles
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 5, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }


        public GameMatcherResponseObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 5, __elementSizes);

            _BattleRoyaleMatchData = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData>(originalBytes, 3, __binaryLastIndex, __tracker);
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (5 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 2, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.BattleRoyaleMatchData>(ref targetBytes, startOffset, offset, 3, _BattleRoyaleMatchData);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 4, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 5, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 5);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameRoomValidationResultFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (1 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>(ref bytes, startOffset, offset, 0, value.ResultEnum);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string[]>(ref bytes, startOffset, offset, 1, value.ProblemPlayersIds);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 1);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameRoomValidationResultObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameRoomValidationResultObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string[]> _ProblemPlayersIds;

        // 0
        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum ResultEnum
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override string[] ProblemPlayersIds
        {
            get
            {
                return _ProblemPlayersIds.Value;
            }
            set
            {
                _ProblemPlayersIds.Value = value;
            }
        }


        public GameRoomValidationResultObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 1, __elementSizes);

            _ProblemPlayersIds = new CacheSegment<TTypeResolver, string[]>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 1, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (1 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string[]>(ref targetBytes, startOffset, offset, 1, ref _ProblemPlayersIds);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 1);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameUnitFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameUnit>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameUnit value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (3 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 0, value.TemporaryId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 1, value.IsBot);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 2, value.PrefabName);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.WarshipCombatPowerLevel);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 3);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameUnit Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameUnitObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameUnitObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameUnit, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 1, 0, 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _PrefabName;

        // 0
        public override int TemporaryId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override bool IsBot
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, bool>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override string PrefabName
        {
            get
            {
                return _PrefabName.Value;
            }
            set
            {
                _PrefabName.Value = value;
            }
        }

        // 3
        public override int WarshipCombatPowerLevel
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 3, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }


        public GameUnitObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 3, __elementSizes);

            _PrefabName = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (3 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 2, ref _PrefabName);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 3);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class CooldownsInfosMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessage value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (1 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, float>(ref bytes, startOffset, offset, 0, value.AbilityMaxCooldown);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo[]>(ref bytes, startOffset, offset, 1, value.WeaponsInfos);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 1);
            }
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new CooldownsInfosMessageObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class CooldownsInfosMessageObjectSegment<TTypeResolver> : global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsInfosMessage, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo[]> _WeaponsInfos;

        // 0
        public override float AbilityMaxCooldown
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, float>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, float>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo[] WeaponsInfos
        {
            get
            {
                return _WeaponsInfos.Value;
            }
            set
            {
                _WeaponsInfos.Value = value;
            }
        }


        public CooldownsInfosMessageObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 1, __elementSizes);

            _WeaponsInfos = new CacheSegment<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo[]>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 1, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (1 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, float>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo[]>(ref targetBytes, startOffset, offset, 1, ref _WeaponsInfos);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 1);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class PositionsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (2 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>>(ref bytes, startOffset, offset, 0, value.EntitiesInfo);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.PlayerEntityId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.Dictionary<int, float>>(ref bytes, startOffset, offset, 2, value.RadiusInfo);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 2);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new PositionsMessageObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class PositionsMessageObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>> _EntitiesInfo;
        CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, float>> _RadiusInfo;

        // 0
        public override global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform> EntitiesInfo
        {
            get
            {
                return _EntitiesInfo.Value;
            }
            set
            {
                _EntitiesInfo.Value = value;
            }
        }

        // 1
        public override int PlayerEntityId
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override global::System.Collections.Generic.Dictionary<int, float> RadiusInfo
        {
            get
            {
                return _RadiusInfo.Value;
            }
            set
            {
                _RadiusInfo.Value = value;
            }
        }


        public PositionsMessageObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 2, __elementSizes);

            _EntitiesInfo = new CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
            _RadiusInfo = new CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, float>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (2 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>>(ref targetBytes, startOffset, offset, 0, ref _EntitiesInfo);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, float>>(ref targetBytes, startOffset, offset, 2, ref _RadiusInfo);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 2);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class TestMessageClass1Formatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1 value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (0 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 0, value.SomeNumber);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 0);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1 Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new TestMessageClass1ObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class TestMessageClass1ObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageClass1, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;


        // 0
        public override int SomeNumber
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, int>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }


        public TestMessageClass1ObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 0, __elementSizes);

        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (0 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 0);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class MessagesContainerFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessagesContainer>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.MessagesContainer value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (0 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, byte[][]>(ref bytes, startOffset, offset, 0, value.Messages);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 0);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.MessagesContainer Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new MessagesContainerObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class MessagesContainerObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Udp.MessagesContainer, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, byte[][]> _Messages;

        // 0
        public override byte[][] Messages
        {
            get
            {
                return _Messages.Value;
            }
            set
            {
                _Messages.Value = value;
            }
        }


        public MessagesContainerObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 0, __elementSizes);

            _Messages = new CacheSegment<TTypeResolver, byte[][]>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (0 + 1));

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, byte[][]>(ref targetBytes, startOffset, offset, 0, ref _Messages);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 0);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class WeaponInfoFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, global::ViewTypeId> formatter0;
        readonly Formatter<TTypeResolver, float> formatter1;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                ;
            }
        }

        public WeaponInfoFormatter()
        {
            formatter0 = Formatter<TTypeResolver, global::ViewTypeId>.Default;
            formatter1 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 8;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 8);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.ViewType);
            offset += formatter1.Serialize(ref bytes, offset, value.Cooldown);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.WeaponInfo(item0, item1);
        }
    }

    public class KillMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        readonly Formatter<TTypeResolver, global::ViewTypeId> formatter1;
        readonly Formatter<TTypeResolver, int> formatter2;
        readonly Formatter<TTypeResolver, global::ViewTypeId> formatter3;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                    && formatter3.NoUseDirtyTracker
                ;
            }
        }

        public KillMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            formatter1 = Formatter<TTypeResolver, global::ViewTypeId>.Default;
            formatter2 = Formatter<TTypeResolver, int>.Default;
            formatter3 = Formatter<TTypeResolver, global::ViewTypeId>.Default;
            
        }

        public override int? GetLength()
        {
            return 16;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 16);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.KillerId);
            offset += formatter1.Serialize(ref bytes, offset, value.KillerType);
            offset += formatter2.Serialize(ref bytes, offset, value.VictimId);
            offset += formatter3.Serialize(ref bytes, offset, value.VictimType);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item2 = formatter2.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item3 = formatter3.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.KillMessage(item0, item1, item2, item3);
        }
    }

    public class PlayerTrackingMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public PlayerTrackingMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.PlayerId);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PlayerTrackingMessage(item0);
        }
    }

    public class PointTrackingMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public PointTrackingMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Point);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.PointTrackingMessage(item0);
        }
    }

    public class ShowPlayerAchievementsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public ShowPlayerAchievementsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.MatchId);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.ShowPlayerAchievementsMessage(item0);
        }
    }

    public class CooldownsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        readonly Formatter<TTypeResolver, float[]> formatter1;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                ;
            }
        }

        public CooldownsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            formatter1 = Formatter<TTypeResolver, float[]>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.AbilityCooldown);
            offset += formatter1.Serialize(ref bytes, offset, value.WeaponsCooldowns);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus.CooldownsMessage(item0, item1);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class ViewTransformFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        readonly Formatter<TTypeResolver, float> formatter1;
        readonly Formatter<TTypeResolver, float> formatter2;
        readonly Formatter<TTypeResolver, global::ViewTypeId> formatter3;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                    && formatter3.NoUseDirtyTracker
                ;
            }
        }

        public ViewTransformFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            formatter1 = Formatter<TTypeResolver, float>.Default;
            formatter2 = Formatter<TTypeResolver, float>.Default;
            formatter3 = Formatter<TTypeResolver, global::ViewTypeId>.Default;
            
        }

        public override int? GetLength()
        {
            return 16;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 16);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.x);
            offset += formatter1.Serialize(ref bytes, offset, value.y);
            offset += formatter2.Serialize(ref bytes, offset, value.angle);
            offset += formatter3.Serialize(ref bytes, offset, value.typeId);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item2 = formatter2.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item3 = formatter3.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform(item0, item1, item2, item3);
        }
    }

    public class Vector2Formatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        readonly Formatter<TTypeResolver, float> formatter1;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                ;
            }
        }

        public Vector2Formatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            formatter1 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 8;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2 value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 8);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.X);
            offset += formatter1.Serialize(ref bytes, offset, value.Y);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2 Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2(item0, item1);
        }
    }

    public class DebugMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, uint> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public DebugMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, uint>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.MessageNumberThatConfirms);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.DebugMessage(item0);
        }
    }

    public class TestMessageStruct1Formatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public TestMessageStruct1Formatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1 value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.SomeNumber);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1 Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestMessageStruct1(item0);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.Common
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class DeliveryConfirmationMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, uint> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public DeliveryConfirmationMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, uint>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.MessageNumberThatConfirms);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.Common.DeliveryConfirmationMessage(item0);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.PlayerToServer
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class BattleExitMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public BattleExitMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.PlayerId);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.PlayerToServer.BattleExitMessage(item0);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class PlayerInputMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        readonly Formatter<TTypeResolver, int> formatter1;
        readonly Formatter<TTypeResolver, float> formatter2;
        readonly Formatter<TTypeResolver, float> formatter3;
        readonly Formatter<TTypeResolver, float> formatter4;
        readonly Formatter<TTypeResolver, bool> formatter5;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                    && formatter3.NoUseDirtyTracker
                    && formatter4.NoUseDirtyTracker
                    && formatter5.NoUseDirtyTracker
                ;
            }
        }

        public PlayerInputMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            formatter1 = Formatter<TTypeResolver, int>.Default;
            formatter2 = Formatter<TTypeResolver, float>.Default;
            formatter3 = Formatter<TTypeResolver, float>.Default;
            formatter4 = Formatter<TTypeResolver, float>.Default;
            formatter5 = Formatter<TTypeResolver, bool>.Default;
            
        }

        public override int? GetLength()
        {
            return 21;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 21);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.TemporaryIdentifier);
            offset += formatter1.Serialize(ref bytes, offset, value.GameRoomNumber);
            offset += formatter2.Serialize(ref bytes, offset, value.X);
            offset += formatter3.Serialize(ref bytes, offset, value.Y);
            offset += formatter4.Serialize(ref bytes, offset, value.Angle);
            offset += formatter5.Serialize(ref bytes, offset, value.UseAbility);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item2 = formatter2.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item3 = formatter3.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item4 = formatter4.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item5 = formatter5.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage(item0, item1, item2, item3, item4, item5);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class PlayerPingMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        readonly Formatter<TTypeResolver, int> formatter1;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                ;
            }
        }

        public PlayerPingMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            formatter1 = Formatter<TTypeResolver, int>.Default;
            
        }

        public override int? GetLength()
        {
            return 8;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 8);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.TemporaryId);
            offset += formatter1.Serialize(ref bytes, offset, value.GameRoomNumber);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage(item0, item1);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.Libraries.NetworkLibrary.Udp.ServerToPlayer
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class HealthPointsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public HealthPointsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.HealthPointsMessage(item0);
        }
    }

    public class MaxHealthPointsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public MaxHealthPointsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxHealthPointsMessage(item0);
        }
    }

    public class MaxShieldPointsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public MaxShieldPointsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.MaxShieldPointsMessage(item0);
        }
    }

    public class ShieldPointsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public ShieldPointsMessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public override global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::Libraries.NetworkLibrary.Udp.ServerToPlayer.ShieldPointsMessage(item0);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class MessageWrapperFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType> formatter0;
        readonly Formatter<TTypeResolver, byte[]> formatter1;
        readonly Formatter<TTypeResolver, uint> formatter2;
        readonly Formatter<TTypeResolver, bool> formatter3;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                    && formatter3.NoUseDirtyTracker
                ;
            }
        }

        public MessageWrapperFormatter()
        {
            formatter0 = Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType>.Default;
            formatter1 = Formatter<TTypeResolver, byte[]>.Default;
            formatter2 = Formatter<TTypeResolver, uint>.Default;
            formatter3 = Formatter<TTypeResolver, bool>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.MessageType);
            offset += formatter1.Serialize(ref bytes, offset, value.SerializedMessage);
            offset += formatter2.Serialize(ref bytes, offset, value.MessageId);
            offset += formatter3.Serialize(ref bytes, offset, value.NeedResponse);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item2 = formatter2.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item3 = formatter3.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.MessageWrapper(item0, item1, item2, item3);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments
{
    using global::System;
    using global::System.Collections.Generic;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;


    public class ViewTypeIdFormatter<TTypeResolver> : Formatter<TTypeResolver, global::ViewTypeId>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::ViewTypeId value)
        {
            return BinaryUtil.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public override global::ViewTypeId Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 4;
            return (global::ViewTypeId)BinaryUtil.ReadInt32(ref bytes, offset);
        }
    }


    public class NullableViewTypeIdFormatter<TTypeResolver> : Formatter<TTypeResolver, global::ViewTypeId?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 5;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::ViewTypeId? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteInt32(ref bytes, offset + 1, (Int32)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 5);
            }

            return 5;
        }

        public override global::ViewTypeId? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 5;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::ViewTypeId)BinaryUtil.ReadInt32(ref bytes, offset + 1);
        }
    }



    public class ViewTypeIdEqualityComparer : IEqualityComparer<global::ViewTypeId>
    {
        public bool Equals(global::ViewTypeId x, global::ViewTypeId y)
        {
            return (Int32)x == (Int32)y;
        }

        public int GetHashCode(global::ViewTypeId x)
        {
            return (int)x;
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http
{
    using global::System;
    using global::System.Collections.Generic;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;


    public class GameRoomValidationResultEnumFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum value)
        {
            return BinaryUtil.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 4;
            return (global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum)BinaryUtil.ReadInt32(ref bytes, offset);
        }
    }


    public class NullableGameRoomValidationResultEnumFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 5;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteInt32(ref bytes, offset + 1, (Int32)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 5);
            }

            return 5;
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 5;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum)BinaryUtil.ReadInt32(ref bytes, offset + 1);
        }
    }



    public class GameRoomValidationResultEnumEqualityComparer : IEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>
    {
        public bool Equals(global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum x, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum y)
        {
            return (Int32)x == (Int32)y;
        }

        public int GetHashCode(global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum x)
        {
            return (int)x;
        }
    }



    public class MatchTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.MatchType>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.MatchType value)
        {
            return BinaryUtil.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.MatchType Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 4;
            return (global::NetworkLibrary.NetworkLibrary.Http.MatchType)BinaryUtil.ReadInt32(ref bytes, offset);
        }
    }


    public class NullableMatchTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.MatchType?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 5;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.MatchType? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteInt32(ref bytes, offset + 1, (Int32)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 5);
            }

            return 5;
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.MatchType? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 5;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::NetworkLibrary.NetworkLibrary.Http.MatchType)BinaryUtil.ReadInt32(ref bytes, offset + 1);
        }
    }



    public class MatchTypeEqualityComparer : IEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.MatchType>
    {
        public bool Equals(global::NetworkLibrary.NetworkLibrary.Http.MatchType x, global::NetworkLibrary.NetworkLibrary.Http.MatchType y)
        {
            return (Int32)x == (Int32)y;
        }

        public int GetHashCode(global::NetworkLibrary.NetworkLibrary.Http.MatchType x)
        {
            return (int)x;
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp
{
    using global::System;
    using global::System.Collections.Generic;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;


    public class MessageTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 1;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.MessageType value)
        {
            return BinaryUtil.WriteSByte(ref bytes, offset, (SByte)value);
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.MessageType Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 1;
            return (global::NetworkLibrary.NetworkLibrary.Udp.MessageType)BinaryUtil.ReadSByte(ref bytes, offset);
        }
    }


    public class NullableMessageTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageType?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 2;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.MessageType? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteSByte(ref bytes, offset + 1, (SByte)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 2);
            }

            return 2;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.MessageType? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 2;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::NetworkLibrary.NetworkLibrary.Udp.MessageType)BinaryUtil.ReadSByte(ref bytes, offset + 1);
        }
    }



    public class MessageTypeEqualityComparer : IEqualityComparer<global::NetworkLibrary.NetworkLibrary.Udp.MessageType>
    {
        public bool Equals(global::NetworkLibrary.NetworkLibrary.Udp.MessageType x, global::NetworkLibrary.NetworkLibrary.Udp.MessageType y)
        {
            return (SByte)x == (SByte)y;
        }

        public int GetHashCode(global::NetworkLibrary.NetworkLibrary.Udp.MessageType x)
        {
             return (int)(SByte)x ^ (int)(SByte)x << 8; 
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
