//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly InvulnerableComponent invulnerableComponent = new InvulnerableComponent();

    public bool isInvulnerable {
        get { return HasComponent(GameComponentsLookup.Invulnerable); }
        set {
            if (value != isInvulnerable) {
                var index = GameComponentsLookup.Invulnerable;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : invulnerableComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherInvulnerable;

    public static Entitas.IMatcher<GameEntity> Invulnerable {
        get {
            if (_matcherInvulnerable == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Invulnerable);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherInvulnerable = matcher;
            }

            return _matcherInvulnerable;
        }
    }
}
