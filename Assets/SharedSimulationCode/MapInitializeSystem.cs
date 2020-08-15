using System.Linq;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using UnityEngine;

namespace SharedSimulationCode
{
    public class MapInitializeSystem:IInitializeSystem
    {
        private readonly Contexts contexts;
        private readonly BattleRoyaleMatchModel matchModel;

        public MapInitializeSystem(Contexts contexts, BattleRoyaleMatchModel matchModel)
        {
            this.contexts = contexts;
            this.matchModel = matchModel;
        }
        
        public void Initialize()
        {
            Vector3 position = new Vector3();
            
            // var firstPlayer = matchModel.GameUnits.Players.First();
            // Debug.LogError($"TemporaryId = "+firstPlayer.TemporaryId);
            // Debug.LogError($"AccountId = "+firstPlayer.AccountId);
            // CreatePlayer(position, firstPlayer.TemporaryId, firstPlayer.AccountId);
            
            foreach (var player in matchModel.GameUnits.Players)
            {
                CreatePlayer(position, player.TemporaryId, player.AccountId);
                position = position + new Vector3(15, 0,15);
            }
            foreach (var botModel in matchModel.GameUnits.Bots)
            {
                var bot = CreatePlayer(position, botModel.TemporaryId, -botModel.TemporaryId);
                bot.isBot = true;
                position = position + new Vector3(15, 0,15);
            }
        }

        private GameEntity CreatePlayer(Vector3 position, ushort playerId, int accountId)
        {
            GameEntity entity = contexts.game.CreateEntity();
            entity.AddPlayer(playerId);
            entity.AddAccount(accountId);
            entity.AddHealthPoints(2000);
            entity.AddMaxHealthPoints(2000);
            entity.AddTeam((byte)(playerId+1));
            entity.AddViewType(ViewTypeId.StarSparrow);
            entity.AddSpawnPosition(position);
            entity.isSpawnWarship = true;
            return entity;
        }
    }
}