//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly ParentFixedComponent parentFixedComponent = new ParentFixedComponent();

    public bool isParentFixed {
        get { return HasComponent(GameComponentsLookup.ParentFixed); }
        set {
            if (value != isParentFixed) {
                var index = GameComponentsLookup.ParentFixed;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : parentFixedComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherParentFixed;

    public static Entitas.IMatcher<GameEntity> ParentFixed {
        get {
            if (_matcherParentFixed == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ParentFixed);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherParentFixed = matcher;
            }

            return _matcherParentFixed;
        }
    }
}
