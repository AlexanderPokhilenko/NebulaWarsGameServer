using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TimeSystems : Feature
{
    public TimeSystems(Contexts contexts) : base("Time Systems")
    {
        Add(new CannonCooldownSubtractionSystem(contexts));
        Add(new LifetimeSubtractionSystem(contexts));
    }
}
