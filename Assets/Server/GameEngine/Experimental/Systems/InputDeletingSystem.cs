using Entitas;

namespace Server.GameEngine.Experimental.Systems
{
    public class InputDeletingSystem : ICleanupSystem, ITearDownSystem
    {
        private readonly ServerInputContext inputContext;

        public InputDeletingSystem(Contexts contexts)
        {
            inputContext = contexts.serverInput;
        }
        
        public void Cleanup()
        {
            inputContext.DestroyAllEntities();
        }

        public void TearDown()
        {
            inputContext.DestroyAllEntities();
        }
    }
}