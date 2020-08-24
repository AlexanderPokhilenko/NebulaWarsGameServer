// using System.Collections.Generic;
// using Entitas;
//
// namespace Server.GameEngine.Experimental.Systems
// {
//     public class UpdatePossibleKillersSystem : ReactiveSystem<ServerGameEntity>, IInitializeSystem
//     {
//         private readonly ServerGameContext gameContext;
//         private readonly Dictionary<int, (int playerId, ViewTypeEnum type)> ownersInfo;
//
//         public UpdatePossibleKillersSystem(Contexts contexts, 
//             Dictionary<int, (int playerId, ViewTypeEnum type)> ownersInfos) 
//             : base(contexts.serverGame)
//         {
//             gameContext = contexts.serverGame;
//             ownersInfo = ownersInfos;
//         }
//
//         public void Initialize()
//         {
//             var zone = gameContext.zone.GetZone(gameContext);
//             ownersInfo.Add(zone.id.value, (0, zone.viewType.value));
//         }
//
//         protected override ICollector<ServerGameEntity> GetTrigger(IContext<ServerGameEntity> context)
//         {
//             return context.CreateCollector(ServerGameMatcher.GrandOwner);
//         }
//
//         protected override bool Filter(ServerGameEntity entity)
//         {
//             return entity.hasGrandOwner;
//         }
//
//         protected override void Execute(List<ServerGameEntity> entities)
//         {
//             foreach (var e in entities)
//             {
//                 var grandOwnerId = e.grandOwner.id;
//                 if (ownersInfo.ContainsKey(grandOwnerId)) return;
//                 var grandOwner = gameContext.GetEntityWithId(grandOwnerId);
//                 if (grandOwner != null && grandOwner.hasViewType)
//                 {
//                     var playerId = 0;
//                     var typeId = grandOwner.viewType.value;
//                     if (grandOwner.hasAccount) playerId = grandOwner.account.id;
//                     ownersInfo.Add(grandOwnerId, (playerId, typeId));
//                 }
//             }
//         }
//     }
// }