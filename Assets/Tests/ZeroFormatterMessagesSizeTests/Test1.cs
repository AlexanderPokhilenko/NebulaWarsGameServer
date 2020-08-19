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
            PositionsMessage positionsMessage = new PositionsMessage()
            {
                entitiesInfo = new Dictionary<ushort, ViewTransform>()
                {
                    {2, new ViewTransform(1, 9, ViewTypeId.Asteroid100)},
                    {1, new ViewTransform(1, 9, ViewTypeId.Asteroid100)},
                    {32, new ViewTransform(1, 9, ViewTypeId.Asteroid100)},
                    {26, new ViewTransform(1, 9, ViewTypeId.Asteroid100)}
                },
                TickNumber = 654
            };

            byte[] serialized = ZeroFormatterSerializer.Serialize(positionsMessage);

            var restored = ZeroFormatterSerializer.Deserialize<PositionsMessage>(serialized);
            
            Assert.AreEqual(positionsMessage.TickNumber, restored.TickNumber);
            CollectionAssert.AreEqual(positionsMessage.entitiesInfo, restored.entitiesInfo);
        }
    }
}