using System;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SharedSimulationCode
{
    public class MapInitializeSystem:IInitializeSystem
    {
        private readonly Contexts contexts;
        private readonly GameObject startSparrowPrefab;
        private readonly BattleRoyaleMatchModel matchModel;

        public MapInitializeSystem(Contexts contexts, BattleRoyaleMatchModel matchModel)
        {
            this.contexts = contexts;
            this.matchModel = matchModel;
            startSparrowPrefab = Resources.Load<GameObject>("Prefabs/StarSparrow1");
            if (startSparrowPrefab == null)
            {
                throw new Exception();
            }
        }
        
        public void Initialize()
        {
            //todo создать корабли
            Vector3 position = new Vector3();
            for (var index = 0; index < matchModel.GameUnits.Players.Count; index++)
            {
                var playerModel = matchModel.GameUnits.Players[index];
                var go = Object.Instantiate(startSparrowPrefab, position, Quaternion.identity);
                GameEntity entity = contexts.game.CreateEntity();
                entity.AddHealthPoints(2000);
                entity.AddMaxHealthPoints(2000);
                entity.AddTeam((byte)(index+1));
                entity.AddViewType(ViewTypeId.StarSparrow);
                entity.AddTransform(go.transform);
                
                
                position = position + new Vector3(10, 0);
            }
        }
    }
}