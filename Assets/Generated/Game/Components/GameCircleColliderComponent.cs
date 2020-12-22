//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CircleColliderComponent circleCollider { get { return (CircleColliderComponent)GetComponent(GameComponentsLookup.CircleCollider); } }
    public bool hasCircleCollider { get { return HasComponent(GameComponentsLookup.CircleCollider); } }

    public void AddCircleCollider(float newRadius) {
        var index = GameComponentsLookup.CircleCollider;
        var component = (CircleColliderComponent)CreateComponent(index, typeof(CircleColliderComponent));
        component.radius = newRadius;
        AddComponent(index, component);
    }

    public void ReplaceCircleCollider(float newRadius) {
        var index = GameComponentsLookup.CircleCollider;
        var component = (CircleColliderComponent)CreateComponent(index, typeof(CircleColliderComponent));
        component.radius = newRadius;
        ReplaceComponent(index, component);
    }

    public void RemoveCircleCollider() {
        RemoveComponent(GameComponentsLookup.CircleCollider);
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

    static Entitas.IMatcher<GameEntity> _matcherCircleCollider;

    public static Entitas.IMatcher<GameEntity> CircleCollider {
        get {
            if (_matcherCircleCollider == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CircleCollider);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCircleCollider = matcher;
            }

            return _matcherCircleCollider;
        }
    }
}