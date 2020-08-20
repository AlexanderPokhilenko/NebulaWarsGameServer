using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
using UnityEngine.TestTools;
using ZeroFormatter;

namespace Tests.ZeroFormatterMessagesSizeTests
{
    public class Test1
    {
        [Test]
        public void Test1SimplePasses()
        {
            // TransformPackMessage positionsMessage = new TransformPackMessage()
            // {
            //     transform = new Dictionary<ushort, ViewTransformCompressed>()
            //     {
            //         {2, new ViewTransformCompressed(1, 9, ViewTypeId.Asteroid100)},
            //         {1, new ViewTransformCompressed(1, 9, ViewTypeId.Asteroid100)},
            //         {32, new ViewTransformCompressed(1, 9, ViewTypeId.Asteroid100)},
            //         {26, new ViewTransformCompressed(1, 9, ViewTypeId.Asteroid100)}
            //     },
            //     TickNumber = 654
            // };
            //
            // byte[] serialized = ZeroFormatterSerializer.Serialize(positionsMessage);
            //
            // var restored = ZeroFormatterSerializer.Deserialize<TransformPackMessage>(serialized);
            //
            // Assert.AreEqual(positionsMessage.TickNumber, restored.TickNumber);
            // CollectionAssert.AreEqual(positionsMessage.transform, restored.transform);
        }
    }
}