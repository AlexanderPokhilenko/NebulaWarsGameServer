//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public DropComponent drop { get { return (DropComponent)GetComponent(GameComponentsLookup.Drop); } }
    public bool hasDrop { get { return HasComponent(GameComponentsLookup.Drop); } }

    public void AddDrop(System.Collections.Generic.List<EntityCreatorObject> newObjects) {
        var index = GameComponentsLookup.Drop;
        var component = (DropComponent)CreateComponent(index, typeof(DropComponent));
        component.objects = newObjects;
        AddComponent(index, component);
    }

    public void ReplaceDrop(System.Collections.Generic.List<EntityCreatorObject> newObjects) {
        var index = GameComponentsLookup.Drop;
        var component = (DropComponent)CreateComponent(index, typeof(DropComponent));
        component.objects = newObjects;
        ReplaceComponent(index, component);
    }

    public void RemoveDrop() {
        RemoveComponent(GameComponentsLookup.Drop);
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

    static Entitas.IMatcher<GameEntity> _matcherDrop;

    public static Entitas.IMatcher<GameEntity> Drop {
        get {
            if (_matcherDrop == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Drop);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDrop = matcher;
            }

            return _matcherDrop;
        }
    }
}
