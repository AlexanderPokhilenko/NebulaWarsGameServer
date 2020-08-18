using SharedSimulationCode.Systems.Sending;

namespace SharedSimulationCode
{
    public class TickCounter:ITickNumberStorage
    {
        private int tickNumber;
        public int GetTickNumber()
        {
            return tickNumber;
        }

        public void AddTick()
        {
            tickNumber++;
        }
    }
}