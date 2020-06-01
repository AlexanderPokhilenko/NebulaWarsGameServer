// using ZeroFormatter.Formatters;
//
// namespace ZeroFormatter.DynamicObjectSegments.NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages
// {
//     public class TestPositionsMessageFormatter<TTypeResolver> : Formatter<TTypeResolver, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestPositionsMessage>
//         where TTypeResolver : ITypeResolver, new()
//     {
//         readonly Formatter<TTypeResolver, global::System.Collections.Generic.Dictionary<ushort, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>> formatter0;
//         readonly Formatter<TTypeResolver, global::System.Collections.Generic.Dictionary<ushort, ushort>> formatter1;
//         
//         public override bool NoUseDirtyTracker
//         {
//             get
//             {
//                 return formatter0.NoUseDirtyTracker
//                        && formatter1.NoUseDirtyTracker
//                     ;
//             }
//         }
//
//         public TestPositionsMessageFormatter()
//         {
//             Formatter.RegisterDictionary<DefaultResolver, ushort, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>();
//             Formatter.RegisterDictionary<DefaultResolver, ushort, ushort>();
//             formatter0 = Formatter<TTypeResolver, System.Collections.Generic.Dictionary<ushort, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.ViewTransform>>.Default;
//             formatter1 = Formatter<TTypeResolver, System.Collections.Generic.Dictionary<ushort, ushort>>.Default;
//             
//         }
//
//         public override int? GetLength()
//         {
//             return null;
//         }
//
//         public override int Serialize(ref byte[] bytes, int offset, global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestPositionsMessage value)
//         {
//             var startOffset = offset;
//             offset += formatter0.Serialize(ref bytes, offset, value.EntitiesInfo);
//             offset += formatter1.Serialize(ref bytes, offset, value.__RadiusInfo);
//             return offset - startOffset;
//         }
//
//         public override global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestPositionsMessage Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
//         {
//             byteSize = 0;
//             int size;
//             var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
//             offset += size;
//             byteSize += size;
//             var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
//             offset += size;
//             byteSize += size;
//             
//             return new global::NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.TestPositionsMessage(item0, item1);
//         }
//     }
// }