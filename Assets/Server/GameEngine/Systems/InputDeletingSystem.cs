﻿using Entitas;

namespace Server.GameEngine.Systems
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