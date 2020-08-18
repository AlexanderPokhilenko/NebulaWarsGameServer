using System.Collections.Generic;
using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using SharedSimulationCode.Physics;
using SharedSimulationCode.Systems;
using SharedSimulationCode.Systems.Check;
using SharedSimulationCode.Systems.Clean;
using SharedSimulationCode.Systems.Cooldown;
using SharedSimulationCode.Systems.Hits;
using SharedSimulationCode.Systems.InputHandling;
using SharedSimulationCode.Systems.MapInitialization;
using SharedSimulationCode.Systems.Sending;
using SharedSimulationCode.Systems.Spawn;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode
{
    /// <summary>
    /// Управляет ecs системами для каждого матча отдельно.
    /// </summary>
    public class MatchSimulation
    {
        public readonly int matchId;
        private readonly Entitas.Systems systems;
        private readonly InputReceiver inputReceiver;
        private readonly TickCounter tickCounter = new TickCounter();

        public MatchSimulation(int matchId, BattleRoyaleMatchModel matchModelArg, UdpSendUtils udpSendUtils, 
            IpAddressesStorage ipAddressesStorage, MatchRemover matchRemover,
            MatchmakerNotifier  matchmakerNotifier)
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
            
            //Создание систем
            systems = new Entitas.Systems()
                    
                    //Создаёт команду спавна игроков
                    .Add(new MapInitializeSystem(contexts, matchModelArg))
                    
                    
                    //Ввод игрока
                    .Add(new MovementSystem(contexts))
                    .Add(new RotationSystem(contexts))
                    .Add(new ShootingSystem(contexts))


                    //Куда это сдвинуть?
                    .Add(new CannonCooldownDecreasingSystem(contexts))


                    //Создаёт GameObj для кораблей
                    .Add(new WarshipsSpawnerSystem(contexts, physicsSpawner))
                    //Создаёт GameObj для снарядов
                    .Add(new ProjectileSpawnerSystem(contexts, physicsSpawner))
                    
                    //До этого места должно быть создание GameObject-ов
                    .Add(new SpawnForceSystem(contexts))

                    //Все создания/пердвижения/удаления gameObj должны произойти до этой системы
                    .Add(new PhysicsSimulateSystem(physicsScene))
                    
                    //Обнаруживает попадания снарядов
                    .Add(new HitDetectionSystem(contexts, physicsRaycaster))
                    .Add(new HitHandlingSystem(contexts))
                    
                    //Отправка текущего состояния мира
                    .Add(new PlayersSendingSystem(matchId, contexts, udpSendUtils))
                    .Add(new TransformSenderSystem(matchId, contexts, udpSendUtils, tickCounter))
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
            tickCounter.AddTick();
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
}