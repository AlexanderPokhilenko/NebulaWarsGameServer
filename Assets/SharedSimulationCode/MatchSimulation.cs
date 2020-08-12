using System;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;

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

        public void AddInputEntity<T>(ushort playerId, Action<InputEntity, T> action, T value)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                if (inputEntity == null)
                {
                    inputEntity = contexts.input.CreateEntity();
                    inputEntity.AddPlayer(playerId);
                }
                action(inputEntity, value);
            }
        }

        public void AddInputEntity(ushort playerId, Action<InputEntity> action)
        {
            if (contexts != null)
            {
                var inputEntity = contexts.input.GetEntityWithPlayer(playerId);
                if (inputEntity == null)
                {
                    inputEntity = contexts.input.CreateEntity();
                    inputEntity.AddPlayer(playerId);
                }
                action(inputEntity);
            }
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