//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AuraComponent aura { get { return (AuraComponent)GetComponent(GameComponentsLookup.Aura); } }
    public bool hasAura { get { return HasComponent(GameComponentsLookup.Aura); } }

    public void AddAura(float newOuterRadius, float newDamage) {
        var index = GameComponentsLookup.Aura;
        var component = (AuraComponent)CreateComponent(index, typeof(AuraComponent));
        component.outerRadius = newOuterRadius;
        component.damage = newDamage;
        AddComponent(index, component);
    }

    public void ReplaceAura(float newOuterRadius, float newDamage) {
        var index = GameComponentsLookup.Aura;
        var component = (AuraComponent)CreateComponent(index, typeof(AuraComponent));
        component.outerRadius = newOuterRadius;
        component.damage = newDamage;
        ReplaceComponent(index, component);
    }

    public void RemoveAura() {
        RemoveComponent(GameComponentsLookup.Aura);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherAura;

    public static Entitas.IMatcher<GameEntity> Aura {
        get {
            if (_matcherAura == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Aura);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAura = matcher;
            }

            return _matcherAura;
        }
    }
}
