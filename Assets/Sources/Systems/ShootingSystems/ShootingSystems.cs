using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ShootingSystems : Feature
{
    public ShootingSystems(Contexts contexts) : base("Shooting Systems")
    {
        Add(new RemovingNegativeCannonCooldownSystem(contexts));
        Add(new CannonShootingSystem(contexts));
        Add(new AddingCannonCooldownSystem(contexts));
        Add(new CannonShootRemoverSystem(contexts));
    }
}
