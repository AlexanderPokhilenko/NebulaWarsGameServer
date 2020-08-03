using Entitas;

namespace Server.GameEngine.Experimental.Systems
{
    public class InputDeletingSystem : ICleanupSystem, ITearDownSystem
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

        public void TearDown()
        {
            inputContext.DestroyAllEntities();
        }
    }
}