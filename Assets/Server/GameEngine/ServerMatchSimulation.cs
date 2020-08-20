using System.Collections.Generic;
using System.Linq;
using NetworkLibrary.NetworkLibrary.Http;
using Plugins.submodules.SharedCode;
using Plugins.submodules.SharedCode.LagCompensation;
using Plugins.submodules.SharedCode.Physics;
using Plugins.submodules.SharedCode.Systems;
using Plugins.submodules.SharedCode.Systems.Check;
using Plugins.submodules.SharedCode.Systems.Clean;
using Plugins.submodules.SharedCode.Systems.Cooldown;
using Plugins.submodules.SharedCode.Systems.Hits;
using Plugins.submodules.SharedCode.Systems.InputHandling;
using Plugins.submodules.SharedCode.Systems.Spawn;
using Server.GameEngine.MatchLifecycle;
using Server.GameEngine.Systems;
using Server.GameEngine.Systems.Sending;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using SharedSimulationCode.LagCompensation;
using SharedSimulationCode.Physics;
using SharedSimulationCode.Systems;
using SharedSimulationCode.Systems.InputHandling;
using SharedSimulationCode.Systems.MapInitialization;
using SharedSimulationCode.Systems.Sending;
using SharedSimulationCode.Systems.Spawn;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Server.GameEngine
{
    /// <summary>
    /// Управляет ecs системами для каждого матча отдельно.
    /// </summary>
    public class ServerMatchSimulation
    {
        public readonly int matchId;
        private readonly Entitas.Systems systems;
        private readonly InputReceiver inputReceiver;
        private readonly IGameStateHistory gameStateHistory = new GameStateHistory();

        public ServerMatchSimulation(int matchId, BattleRoyaleMatchModel matchModelArg, UdpSendUtils udpSendUtils, 
            IpAddressesStorage ipAddressesStorage, MatchRemover matchRemover,
            MatchmakerNotifier  matchmakerNotifier, ITickDeltaTimeStorage tickDeltaTimeStorage)
        {
            //Создание физической сцены для комнаты
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
            Scene matchScene = SceneManager.LoadScene("EmptyScene", loadSceneParameters);
            var physicsScene = matchScene.GetPhysicsScene();
            PhysicsSpawner physicsSpawner = new PhysicsSpawner(matchScene);
            PhysicsRaycaster physicsRaycaster = new PhysicsRaycaster(matchScene);
            PhysicsDestroyer physicsDestroyer = new PhysicsDestroyer();
                
            //Создание разных штук
            this.matchId = matchId;
            var playerDeathHandler = new PlayerDeathHandler(matchmakerNotifier, udpSendUtils, ipAddressesStorage);
            var contexts = new Contexts();

            List<ushort> playerTmpIds = matchModelArg.GetPlayerTmpIds();
            int maxInputLength = 60;
            InputMessagesMetaHistory history = new InputMessagesMetaHistory(maxInputLength, playerTmpIds);
            inputReceiver = new InputReceiver(contexts, history);
            
            //Автоматическое добавление id при создании сущности
            contexts.SubscribeId();
            
            
            ArrangeTransformSystem[] arrangeCollidersSystems = {
                new WarshipsArrangeTransformSystem(contexts)
            };
            ITimeMachine timeMachine = new TimeMachine(gameStateHistory, arrangeCollidersSystems);
            LagCompensationSystem[] lagCompensationSystems = {
                new HitDetectionSystem(contexts, physicsRaycaster, tickDeltaTimeStorage), 
            }; 
            
            
            
            systems = new Entitas.Systems()
                    //Создаёт новое состояние
                    .Add(new GameStateHistoryUpdaterSystem(contexts, gameStateHistory))
                    //Создаёт команду спавна игроков
                    .Add(new MapInitializeSystem(contexts, matchModelArg))
                    
                    
                    //Ввод игрока
                    .Add(new MoveSystem(contexts))
                    .Add(new RotationSystem(contexts))
                    .Add(new ShootingSystem(contexts))


                    //Куда это сдвинуть?
                    .Add(new CannonCooldownDecreasingSystem(contexts, tickDeltaTimeStorage))


                    //Создаёт GameObj для кораблей
                    .Add(new WarshipsSpawnerSystem(contexts, physicsSpawner))
                    //Создаёт GameObj для снарядов
                    .Add(new ProjectileSpawnerSystem(contexts, physicsSpawner))
                    
                    //До этого места должно быть создание GameObject-ов
                    .Add(new SpawnForceSystem(contexts))

                    //Все создания/пердвижения gameObj должны произойти до этой системы
                    .Add(new PhysicsSimulateSystem(physicsScene, tickDeltaTimeStorage))
                    
                    
                    //неведомая дичь
                    .Add(new LagCompensationSystemGroup(contexts, timeMachine, lagCompensationSystems, gameStateHistory))


                    .Add(new ProjectileTickNumberUpdaterSystem(contexts))
                    
                    //Обнаруживает попадания снарядов
                    .Add(new HitHandlingSystem(contexts))
                    
                    
                    //Отправка текущего состояния мира
                    .Add(new PlayersSendingSystem(matchId, contexts, udpSendUtils))
                    .Add(new TransformSenderSystem(matchId, contexts, udpSendUtils, gameStateHistory))
                    .Add(new HealthSenderSystem(contexts, matchId, udpSendUtils))
                    .Add(new MaxHealthSenderSystem(contexts, matchId, udpSendUtils))
                    
                    //Очистка
                    .Add(new InputClearSystem(contexts))
                    .Add(new DestroyEntitySystem(contexts, physicsDestroyer))
                    //Проверки
                    .Add(new PositionCheckSystem(contexts))
                    
                ;
        }

        public void Initialize()
        {
            systems.Initialize();
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

        public InputReceiver GetInputReceiver()
        {
            return inputReceiver;
        }
    }

    public class GameStateHistory : IGameStateHistory
    {
        private Dictionary<int, ServerGameState> history = new Dictionary<int, ServerGameState>();
        public ServerGameState Get(int tickNumber)
        {
            return history[tickNumber];
        }

        public int GetLastTickNumber()
        {
            return history.Keys.Max();
        }

        public void Add(ServerGameState serverGameState)
        {
            history.Add(serverGameState.tickNumber, serverGameState);
        }

        public ServerGameState GetActualGameState()
        {
            return history[GetLastTickNumber()];
        }
    }
}