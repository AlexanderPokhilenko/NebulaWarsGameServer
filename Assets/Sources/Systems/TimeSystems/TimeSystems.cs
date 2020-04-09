internal class TimeSystems : Feature
{
    public TimeSystems(Contexts contexts) : base("Time Systems")
    {
        Add(new CannonCooldownSubtractionSystem(contexts));
        Add(new AbilityCooldownSubtractionSystem(contexts));
        Add(new LifetimeSubtractionSystem(contexts));
    }
}
