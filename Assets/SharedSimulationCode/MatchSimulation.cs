using System;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using UnityEngine;
using Vector2 = NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages.Vector2;

namespace SharedSimulationCode
{
    public class MatchSimulation
    {
        public readonly int matchId;
        private readonly Systems systems;
        private readonly Contexts contexts;

        public MatchSimulation(int matchId, BattleRoyaleMatchModel matchModelArg, UdpSendUtils udpSendUtils, 
            IpAddressesStorage ipAddressesStorage, MatchRemover matchRemover,
            MatchmakerNotifier  matchmakerNotifier)
        {
            this.matchId = matchId;
            var playerDeathHandler = new PlayerDeathHandler(matchmakerNotifier, udpSendUtils, ipAddressesStorage);
            contexts = new Contexts();
            contexts.SubscribeId();
            systems = new Systems()
                    .Add(new MapInitializeSystem(contexts, matchModelArg))
                    .Add(new PositionSenderSystem(matchId, contexts, udpSendUtils))
                    // .Add(new PositionSenderSystem(contexts))
                ;
            
            //todo нужно оповестить игроков о связи accountId - entityId
        }
        

        public void Initialize()
        {
            systems.Initialize();
        }

        public void AddMovement(ushort playerId, Vector2 vector2)
        {
            if (contexts != null)
            {
                var inputEntity = GetEntityForPlayer(playerId);
                inputEntity.AddMovement(vector2);
            }
        }
        public void AddAttack(ushort playerId, float attackAngle)
        {
            if (contexts != null)
            {
                var inputEntity = GetEntityForPlayer(playerId);
                inputEntity.AddAttack(attackAngle);
            }
        }
        
        public void AddExit(ushort playerId)
        {
            if (contexts != null)
            {
                var inputEntity = GetEntityForPlayer(playerId);
                inputEntity.isLeftTheGame = true;
            }
        }
        
        public void AddAbility(ushort playerId)
        {
            if (contexts != null)
            {
                var inputEntity = GetEntityForPlayer(playerId);
                inputEntity.isTryingToUseAbility = true;
            }
        }

        private InputEntity GetEntityForPlayer(ushort playerId)
        {
            var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
            if (inputEntity == null)
            {
                inputEntity = contexts.input.CreateEntity();
                inputEntity.AddPlayer(playerId);
            }

            return inputEntity;
        }

      
        
        public void Tick()
        {
            systems.Execute();
            systems.Cleanup();
        }

        public void TearDown()
        {
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
        }
    }
}