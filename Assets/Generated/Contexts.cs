//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : Entitas.IContexts {

    public static Contexts sharedInstance {
        get {
            if (_sharedInstance == null) {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public GameContext game { get; set; }
    public InputContext input { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { game, input }; } }

    public Contexts() {
        game = new GameContext();
        input = new InputContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors) {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset() {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++) {
            contexts[i].Reset();
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EntityIndexGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

    public const string Account = "Account";
    public const string GrandOwner = "GrandOwner";
    public const string Id = "Id";
    public const string KilledBy = "KilledBy";
    public const string Owner = "Owner";
    public const string Parent = "Parent";
    public const string Player = "Player";
    public const string Team = "Team";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
        game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, int>(
            Account,
            game.GetGroup(GameMatcher.Account),
            (e, c) => ((AccountComponent)c).id));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, ushort>(
            GrandOwner,
            game.GetGroup(GameMatcher.GrandOwner),
            (e, c) => ((GrandOwnerComponent)c).id));

        game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, ushort>(
            Id,
            game.GetGroup(GameMatcher.Id),
            (e, c) => ((IdComponent)c).value));
        input.AddEntityIndex(new Entitas.PrimaryEntityIndex<InputEntity, ushort>(
            Id,
            input.GetGroup(InputMatcher.Id),
            (e, c) => ((IdComponent)c).value));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, int>(
            KilledBy,
            game.GetGroup(GameMatcher.KilledBy),
            (e, c) => ((KilledByComponent)c).id));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, ushort>(
            Owner,
            game.GetGroup(GameMatcher.Owner),
            (e, c) => ((OwnerComponent)c).id));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, ushort>(
            Parent,
            game.GetGroup(GameMatcher.Parent),
            (e, c) => ((ParentComponent)c).id));

        game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, ushort>(
            Player,
            game.GetGroup(GameMatcher.Player),
            (e, c) => ((PlayerComponent)c).id));
        input.AddEntityIndex(new Entitas.PrimaryEntityIndex<InputEntity, ushort>(
            Player,
            input.GetGroup(InputMatcher.Player),
            (e, c) => ((PlayerComponent)c).id));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, byte>(
            Team,
            game.GetGroup(GameMatcher.Team),
            (e, c) => ((TeamComponent)c).id));
    }
}

public static class ContextsExtensions {

    public static GameEntity GetEntityWithAccount(this GameContext context, int id) {
        return ((Entitas.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Account)).GetEntity(id);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithGrandOwner(this GameContext context, ushort id) {
        return ((Entitas.EntityIndex<GameEntity, ushort>)context.GetEntityIndex(Contexts.GrandOwner)).GetEntities(id);
    }

    public static GameEntity GetEntityWithId(this GameContext context, ushort value) {
        return ((Entitas.PrimaryEntityIndex<GameEntity, ushort>)context.GetEntityIndex(Contexts.Id)).GetEntity(value);
    }

    public static InputEntity GetEntityWithId(this InputContext context, ushort value) {
        return ((Entitas.PrimaryEntityIndex<InputEntity, ushort>)context.GetEntityIndex(Contexts.Id)).GetEntity(value);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithKilledBy(this GameContext context, int id) {
        return ((Entitas.EntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.KilledBy)).GetEntities(id);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithOwner(this GameContext context, ushort id) {
        return ((Entitas.EntityIndex<GameEntity, ushort>)context.GetEntityIndex(Contexts.Owner)).GetEntities(id);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithParent(this GameContext context, ushort id) {
        return ((Entitas.EntityIndex<GameEntity, ushort>)context.GetEntityIndex(Contexts.Parent)).GetEntities(id);
    }

    public static GameEntity GetEntityWithPlayer(this GameContext context, ushort id) {
        return ((Entitas.PrimaryEntityIndex<GameEntity, ushort>)context.GetEntityIndex(Contexts.Player)).GetEntity(id);
    }

    public static InputEntity GetEntityWithPlayer(this InputContext context, ushort id) {
        return ((Entitas.PrimaryEntityIndex<InputEntity, ushort>)context.GetEntityIndex(Contexts.Player)).GetEntity(id);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithTeam(this GameContext context, byte id) {
        return ((Entitas.EntityIndex<GameEntity, byte>)context.GetEntityIndex(Contexts.Team)).GetEntities(id);
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.VisualDebugging.CodeGeneration.Plugins.ContextObserverGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeContextObservers() {
        try {
            CreateContextObserver(game);
            CreateContextObserver(input);
        } catch(System.Exception) {
        }
    }

    public void CreateContextObserver(Entitas.IContext context) {
        if (UnityEngine.Application.isPlaying) {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }

#endif
}
