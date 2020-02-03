using System;
using System.Collections.Concurrent;
using System.Threading;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEditor;
using UnityEngine;

namespace AmoebaBattleServer01.Experimental.GameEngine.Systems
{
    public class InputDeletingSystem : ICleanupSystem
    {
        private readonly InputContext inputContext;

        public InputDeletingSystem(Contexts contexts)
        {
            inputContext = contexts.input;
        }
        
        public void Cleanup()
        {
            inputContext.DestroyAllEntities();
        }
    }
}