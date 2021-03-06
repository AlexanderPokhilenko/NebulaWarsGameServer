//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly DirectionTargetingShootingComponent directionTargetingShootingComponent = new DirectionTargetingShootingComponent();

    public bool isDirectionTargetingShooting {
        get { return HasComponent(GameComponentsLookup.DirectionTargetingShooting); }
        set {
            if (value != isDirectionTargetingShooting) {
                var index = GameComponentsLookup.DirectionTargetingShooting;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : directionTargetingShootingComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherDirectionTargetingShooting;

    public static Entitas.IMatcher<GameEntity> DirectionTargetingShooting {
        get {
            if (_matcherDirectionTargetingShooting == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.DirectionTargetingShooting);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDirectionTargetingShooting = matcher;
            }

            return _matcherDirectionTargetingShooting;
        }
    }
}
