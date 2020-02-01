using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AnimationSystems : Feature
{
    public AnimationSystems(Contexts contexts) : base("Animation Systems")
    {
        Add(new SetAnimatorSystem(contexts));
        Add(new MovingAnimationSystem(contexts));
        Add(new DeathAnimationSystem(contexts));
        Add(new DyingAnimationEndingSystem(contexts));
    }
}
