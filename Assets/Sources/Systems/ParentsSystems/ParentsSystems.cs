using Entitas;

public class ParentsSystems : Feature
{
    public ParentsSystems(Contexts contexts) : base("Parents Systems")
    {
        Add(new ParentsRecursionPreventionSystem(contexts));
        Add(new ParentFixedCheckerSystem(contexts));
        Add(new ParentFixedRemoverSystem(contexts));
        Add(new UpdateGrandOwnerSystem(contexts));
    }
}