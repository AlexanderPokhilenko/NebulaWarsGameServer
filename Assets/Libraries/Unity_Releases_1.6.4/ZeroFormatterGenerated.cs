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
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnumFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnumEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum?>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.NullableGameRoomValidationResultEnumFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum?>.Register(new NullableEqualityComparer<global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultEnum>());
            
            // Objects
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoomFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomDataFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameMatherResponse>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameMatherResponseFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResult>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Http.GameRoomValidationResultFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessage>.Register(new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.PositionsMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            // Structs
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TransformFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2Formatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.Ping.PlayerPingMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessageFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message>(structFormatter));
            }
            {
                var structFormatter = new ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.MessageContainerFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer>(structFormatter));
            }
            // Unions
            // Generics
            ZeroFormatter.Formatters.Formatter.RegisterDictionary<ZeroFormatter.Formatters.DefaultResolver, int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message>();
            ZeroFormatter.Formatters.Formatter.RegisterArray<ZeroFormatter.Formatters.DefaultResolver, string>();
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

    public class PlayerInfoForGameRoomFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom value)
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
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 0, value.GoogleId);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.TemporaryIdentifier);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 1);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new PlayerInfoForGameRoomObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class PlayerInfoForGameRoomObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _GoogleId;

        // 0
        public override string GoogleId
        {
            get
            {
                return _GoogleId.Value;
            }
            set
            {
                _GoogleId.Value = value;
            }
        }

        // 1
        public override int TemporaryIdentifier
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


        public PlayerInfoForGameRoomObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 1, __elementSizes);

            _GoogleId = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
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

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 0, ref _GoogleId);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 1);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameRoomDataFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData value)
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
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 1, value.GameRoomNumber);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom[]>(ref bytes, startOffset, offset, 2, value.Players);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, int>(ref bytes, startOffset, offset, 3, value.GameServerPort);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 3);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomData Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameRoomDataObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameRoomDataObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameRoomData, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 4, 0, 4 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _GameServerIp;
        CacheSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom[]> _Players;

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
        public override int GameRoomNumber
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
        public override global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom[] Players
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

        // 3
        public override int GameServerPort
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


        public GameRoomDataObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 3, __elementSizes);

            _GameServerIp = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
            _Players = new CacheSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom[]>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
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
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.PlayerInfoForGameRoom[]>(ref targetBytes, startOffset, offset, 2, ref _Players);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, int>(ref targetBytes, startOffset, offset, 3, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 3);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }

    public class GameMatherResponseFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameMatherResponse>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Http.GameMatherResponse value)
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
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 0, value.PlayerHasJustBeenRegistered);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 1, value.PlayerInQueue);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, bool>(ref bytes, startOffset, offset, 2, value.PlayerInBattle);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData>(ref bytes, startOffset, offset, 3, value.GameRoomData);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 3);
            }
        }

        public override global::NetworkLibrary.NetworkLibrary.Http.GameMatherResponse Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new GameMatherResponseObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class GameMatherResponseObjectSegment<TTypeResolver> : global::NetworkLibrary.NetworkLibrary.Http.GameMatherResponse, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 1, 1, 1, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        global::NetworkLibrary.NetworkLibrary.Http.GameRoomData _GameRoomData;

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
        public override global::NetworkLibrary.NetworkLibrary.Http.GameRoomData GameRoomData
        {
            get
            {
                return _GameRoomData;
            }
            set
            {
                __tracker.Dirty();
                _GameRoomData = value;
            }
        }


        public GameMatherResponseObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 3, __elementSizes);

            _GameRoomData = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData>(originalBytes, 3, __binaryLastIndex, __tracker);
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

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, bool>(ref targetBytes, startOffset, offset, 2, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Http.GameRoomData>(ref targetBytes, startOffset, offset, 3, _GameRoomData);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 3);
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

                offset += (8 + 4 * (0 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>>(ref bytes, startOffset, offset, 0, value.EntitiesInfo);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 0);
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
        static readonly int[] __elementSizes = new int[]{ 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>> _EntitiesInfo;

        // 0
        public override global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform> EntitiesInfo
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


        public PositionsMessageObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 0, __elementSizes);

            _EntitiesInfo = new CacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 0, __binaryLastIndex, __tracker));
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

                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, global::System.Collections.Generic.Dictionary<int, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>>(ref targetBytes, startOffset, offset, 0, ref _EntitiesInfo);

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
namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class TransformFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, float> formatter0;
        readonly Formatter<TTypeResolver, float> formatter1;
        readonly Formatter<TTypeResolver, float> formatter2;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                ;
            }
        }

        public TransformFormatter()
        {
            formatter0 = Formatter<TTypeResolver, float>.Default;
            formatter1 = Formatter<TTypeResolver, float>.Default;
            formatter2 = Formatter<TTypeResolver, float>.Default;
            
        }

        public override int? GetLength()
        {
            return 12;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 12);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.x);
            offset += formatter1.Serialize(ref bytes, offset, value.y);
            offset += formatter2.Serialize(ref bytes, offset, value.angle);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
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
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Transform(item0, item1, item2);
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
            offset += formatter0.Serialize(ref bytes, offset, value.PlayerTemporaryIdentifierForTheMatch);
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
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                    && formatter3.NoUseDirtyTracker
                    && formatter4.NoUseDirtyTracker
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
            
        }

        public override int? GetLength()
        {
            return 20;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 20);
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.TemporaryIdentifier);
            offset += formatter1.Serialize(ref bytes, offset, value.GameRoomNumber);
            offset += formatter2.Serialize(ref bytes, offset, value.X);
            offset += formatter3.Serialize(ref bytes, offset, value.Y);
            offset += formatter4.Serialize(ref bytes, offset, value.Angle);
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
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage.PlayerInputMessage(item0, item1, item2, item3, item4);
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

    public class MessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, int> formatter0;
        readonly Formatter<TTypeResolver, byte[]> formatter1;
        readonly Formatter<TTypeResolver, int> formatter2;
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

        public MessageFormatter()
        {
            formatter0 = Formatter<TTypeResolver, int>.Default;
            formatter1 = Formatter<TTypeResolver, byte[]>.Default;
            formatter2 = Formatter<TTypeResolver, int>.Default;
            formatter3 = Formatter<TTypeResolver, bool>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.Message value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.MessageType);
            offset += formatter1.Serialize(ref bytes, offset, value.SerializedMessage);
            offset += formatter2.Serialize(ref bytes, offset, value.MessageId);
            offset += formatter3.Serialize(ref bytes, offset, value.NeedResponse);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.Message Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
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
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.Message(item0, item1, item2, item3);
        }
    }

    public class MessageContainerFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message[]> formatter0;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                ;
            }
        }

        public MessageContainerFormatter()
        {
            formatter0 = Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.Message[]>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.Messages);
            return offset - startOffset;
        }

        public override global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::NetworkLibrary.NetworkLibrary.Udp.MessageContainer(item0);
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



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
