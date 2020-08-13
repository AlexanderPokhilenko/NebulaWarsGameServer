using System;
using System.Linq;
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
            Vector3 position = new Vector3();
            var firstPlayer = matchModel.GameUnits.Players.First();
            CreatePlayer(position, firstPlayer.TemporaryId, firstPlayer.AccountId);
            
            // foreach (var player in matchModel.GameUnits.Players)
            // {
            //     CreatePlayer(position, player.TemporaryId, player.AccountId);
            //     position = position + new Vector3(15, 0,15);
            // }
            // foreach (var botModel in matchModel.GameUnits.Bots)
            // {
            //     CreateBot(position, botModel.TemporaryId, -botModel.TemporaryId);
            //     position = position + new Vector3(15, 0,15);
            // }
        }

        private void CreatePlayer(Vector3 position, ushort playerId, int accountId)
        {
            var go = Object.Instantiate(startSparrowPrefab, position, Quaternion.identity);
            GameEntity entity = contexts.game.CreateEntity();
            
            entity.AddPlayer(playerId);
            entity.AddAccount(accountId);
            entity.AddHealthPoints(2000);
            entity.AddMaxHealthPoints(2000);
            entity.AddTeam((byte)(playerId+1));
            entity.AddViewType(ViewTypeId.StarSparrow);
            entity.AddTransform(go.transform);
            var rigidbody = go.GetComponent<Rigidbody>();
            entity.AddRigidbody(rigidbody);
        }
        
        private void CreateBot(Vector3 position, ushort playerId, int accountId)
        {
            var go = Object.Instantiate(startSparrowPrefab, position, Quaternion.identity);
            GameEntity entity = contexts.game.CreateEntity();
            entity.AddPlayer(playerId);
            entity.AddAccount(accountId);
            entity.isBot = true;
            entity.AddHealthPoints(2000);
            entity.AddMaxHealthPoints(2000);
            entity.AddTeam((byte)(playerId+1));
            entity.AddViewType(ViewTypeId.StarSparrow);
            entity.AddTransform(go.transform);
            var rigidbody = go.GetComponent<Rigidbody>();
            entity.AddRigidbody(rigidbody);
        }
    }
}