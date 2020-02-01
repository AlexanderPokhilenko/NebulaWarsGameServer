using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class EffectsSystems : Feature
{
    public EffectsSystems(Contexts contexts) : base("Effects Systems")
    {
        Add(new BonusApplyingSystem(contexts));
        Add(new AuraDamageSystem(contexts));
        Add(new CircleScalingSystem(contexts));
    }
}
