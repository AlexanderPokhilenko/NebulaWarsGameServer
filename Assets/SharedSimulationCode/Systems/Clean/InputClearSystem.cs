using Entitas;

namespace SharedSimulationCode.Systems.Clean
{
    public class InputClearSystem:ICleanupSystem
    {
        private readonly InputContext inputContext;

        public InputClearSystem(Contexts contexts)
        {
            inputContext = contexts.input;
        }
        
        public void Cleanup()
        {
            inputContext.DestroyAllEntities();
        }
    }
}