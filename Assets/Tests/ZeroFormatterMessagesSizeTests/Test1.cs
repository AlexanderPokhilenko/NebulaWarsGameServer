// using System.Collections.Generic;
// using NUnit.Framework;
// using Plugins.submodules.SharedCode.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
// using Plugins.submodules.SharedCode.Systems.InputHandling;
// using UnityEngine;
// using ZeroFormatter;
//
// namespace Tests.ZeroFormatterMessagesSizeTests
// {
//     public class VectorUtilsTest
//     {
//         private Vector3Utils vector3Utils;
//
//         public VectorUtilsTest()
//         {
//             vector3Utils = new Vector3Utils();
//         }
//         
//         [Test]
//         public void Test1()
//         {
//             //arrange
//             List<Vector3> inputVectors = new List<Vector3>()
//             {
//                 Vector3.forward,
//                 Vector3.zero,
//                 Vector3.zero,
//                 Vector3.zero,
//                 Vector3.zero,
//                 Vector3.zero,
//             };
//             
//             //act
//             Vector3 result = vector3Utils.GetVelocityVector(inputVectors);
//             
//             //assert
//             Vector3 expected = new Vector3(0,0,0.1666667F);
//             Vector3 delta = result - expected;
//             float tolerance = 0.01f;
//             Assert.GreaterOrEqual(tolerance, delta.magnitude);
//         }
//     }
//     public class Test1
//     {
//         [Test]
//         public void Test1SimplePasses()
//         {
//             float originalX = 100;
//             float originalZ = 100;
//             float originalAngle = 100;
//             ViewTypeEnum originalViewTypeEnum = ViewTypeEnum.Raven;
//             
//             ViewTransformCompressed viewTransform = new ViewTransformCompressed(originalX, originalZ, originalAngle, originalViewTypeEnum);
//
//             Vector3 vector3 = viewTransform.GetPosition();
//
//             float tolerance = 0.01f;
//             float deltaX = Mathf.Abs(originalX - vector3.x);
//             if (deltaX > tolerance)
//             {
//                 Assert.Fail($"Слишком большая погрешность x {deltaX}");
//             }
//             
//             
//             if (vector3.y > tolerance)
//             {
//                 Assert.Fail($"Слишком большая погрешность y {deltaX}");
//             }
//
//             float deltaZ = Mathf.Abs(originalZ - vector3.z);
//             if (deltaZ > tolerance)
//             {
//                 Assert.Fail($"Слишком большая погрешность x {deltaZ}");
//             }
//
//             Debug.LogWarning($"{vector3.x} {vector3.y} {vector3.z}");
//         }
//         
//         [Test]
//         public void Test2()
//         {
//             float tolerance = 0.01f;
//             var dictionary = new Dictionary<ushort, ViewTransformCompressed>();
//             for (ushort i = 0; i <= 20; i++)
//             {
//                 float originalX = i*4+10;
//                 float originalZ = 100-i*20;
//                 float originalAngle = i*Mathf.PI;
//                 ViewTypeEnum originalViewTypeEnum = ViewTypeEnum.Raven;    
//                 var viewTransform = new ViewTransformCompressed(originalX, originalZ, originalAngle, originalViewTypeEnum);
//                 dictionary.Add(i, viewTransform);
//             }
//
//             TransformPackMessage message = new TransformPackMessage(dictionary, 0, 0, 0);
//             byte[] data = ZeroFormatterSerializer.Serialize(message);
//             TransformPackMessage restored = ZeroFormatterSerializer.Deserialize<TransformPackMessage>(data);
//
//             foreach (var pair in restored.transform)
//             {
//                 ushort id = pair.Key;
//                 var viewTransformCompressedRestored = pair.Value;
//                 var viewTransformCompressedOriginal = dictionary[id];
//                 Vector3 position = viewTransformCompressedRestored.GetPosition();
//                 float deltaX = Mathf.Abs(viewTransformCompressedOriginal.X - position.x);
//                 float deltaZ = Mathf.Abs(viewTransformCompressedOriginal.Z - position.z);
//                 float deltaAngle = Mathf.Abs(viewTransformCompressedOriginal.Angle - viewTransformCompressedRestored.Angle);
//
//                 if (tolerance < deltaX )
//                 {
//                     Assert.Fail($"deltaX={deltaX} {viewTransformCompressedOriginal.X} {position.x}");
//                 }
//                 
//                 if (tolerance < deltaZ)
//                 {
//                     Assert.Fail($"deltaZ={deltaZ}");
//                 }
//                 
//                 if (tolerance < deltaAngle)
//                 {
//                     Assert.Fail($"deltaAngle={deltaAngle}");
//                 }
//             }
//         }
//         
//         [Test]
//         public void Test3()
//         {
//             var original = new ViewTransformCompressed(10,10,10,ViewTypeEnum.Raven);
//             byte[] bytes = ZeroFormatterSerializer.Serialize(original);
//             var restored = ZeroFormatterSerializer.Deserialize<ViewTransformCompressed>(bytes);
//             
//             float tolerance = 0.01f;
//             float deltaX = Mathf.Abs(original.X - restored.X);
//             float deltaZ = Mathf.Abs(original.Z - restored.Z);
//             float deltaAngle = Mathf.Abs(original.Angle - restored.Angle);
//
//             if (tolerance < deltaX )
//             {
//                 Assert.Fail($"deltaX={deltaX} {original.X} {restored.X}");
//             }
//                 
//             if (tolerance < deltaZ)
//             {
//                 Assert.Fail($"deltaZ={deltaZ} {original.Z} {restored.Z}");
//             }
//                 
//             if (tolerance < deltaAngle)
//             {
//                 Assert.Fail($"deltaAngle={deltaAngle} {original.Angle} {restored.Angle}");
//             }
//         }
//
//
//         [Test]
//         public void TestViewTransformCompressed()
//         {
//             var original = new ViewTransformCompressed(-10, 10, 20, ViewTypeEnum.Raven);
//             Debug.LogWarning($"{original.X} {original.Z} {original.Angle}");
//             byte[] data = ZeroFormatterSerializer.Serialize(original);
//             var restored = ZeroFormatterSerializer.Deserialize<ViewTransformCompressed>(data);
//             Debug.LogWarning($"{restored.X} {restored.Z} {restored.Angle}");
//         }
//         
//         [Test]
//         public void TestViewTransform2()
//         {
//             var original = new ViewTransformCompressed();
//             original.__x = Mathf.FloatToHalf(-10);
//             original.__z = Mathf.FloatToHalf(10);
//             original.__angle = Mathf.FloatToHalf(20);
//             Debug.LogWarning($"{original.X} {original.Z} {original.Angle}");
//             byte[] data = ZeroFormatterSerializer.Serialize(original);
//             var restored = ZeroFormatterSerializer.Deserialize<ViewTransformCompressed>(data);
//             Debug.LogWarning($"{restored.X} {restored.Z} {restored.Angle}");
//         }
//      
//         
//         [Test]
//         public void Test54()
//         {
//             var original = new ViewTransformCompressed(-100, 100, 200, ViewTypeEnum.Raven);
//             Debug.LogWarning($"{original.X} {original.Z} {original.Angle}");
//             byte[] data = ZeroFormatterSerializer.Serialize(original);
//             var restored = ZeroFormatterSerializer.Deserialize<ViewTransformCompressed>(data);
//             Debug.LogWarning($"{restored.X} {restored.Z} {restored.Angle}");
//         }
//     }
// }