using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class DestroySystems : Feature
{
    public DestroySystems(Contexts contexts) : base("Destroy Systems")
    {
        Add(new LowHealthSystem(contexts));
        Add(new LowLifetimeSystem(contexts));
        Add(new CollapsingSystem(contexts));
        Add(new ParentLinkDeletingSystem(contexts));
        Add(new TargetLinkDeletingSystem(contexts));
        Add(new DeletingEntitiesWithoutAnimatorSystem(contexts));
        Add(new DeleteSystem(contexts));
    }
}
