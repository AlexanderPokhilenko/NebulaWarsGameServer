using Entitas;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.MatchLifecycle;
using Server.Http;
using Server.Udp.Sending;
using Server.Udp.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SharedSimulationCode
{
    public class MatchSimulation
    {
        public readonly int matchId;
        private readonly Systems systems;
        private readonly InputReceiver inputReceiver;

        public MatchSimulation(int matchId, BattleRoyaleMatchModel matchModelArg, UdpSendUtils udpSendUtils, 
            IpAddressesStorage ipAddressesStorage, MatchRemover matchRemover,
            MatchmakerNotifier  matchmakerNotifier)
        {
            
            //Создание физической сцены для комнаты
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
            Scene matchScene = SceneManager.LoadScene("EmptyScene", loadSceneParameters);
            var physicsScene = matchScene.GetPhysicsScene();
            PhysicsRaycaster physicsRaycaster = new PhysicsRaycaster(physicsScene);
            
                
            //Создание разных штук
            this.matchId = matchId;
            var playerDeathHandler = new PlayerDeathHandler(matchmakerNotifier, udpSendUtils, ipAddressesStorage);
            var contexts = new Contexts();
            var physicsSpawner = new PhysicsSpawner(matchScene);
            inputReceiver = new InputReceiver(contexts);
            
            //Автоматическое добавление id при создании сущности
            contexts.SubscribeId();
            
            //Создание систем
            systems = new Systems()
                    .Add(new MapInitializeSystem(contexts, matchModelArg))
                    .Add(new PlayersSendingSystem(matchId, contexts, udpSendUtils))


                    .Add(new ShootingSystem(contexts))
                    
                    
                    .Add(new WarshipsSpawnerSystem(contexts, physicsSpawner))
                    .Add(new SpawnForceSystem(contexts))
                    
                    .Add(new MovementSystem(contexts))
                    .Add(new RotationSystem(contexts))
                    
                    .Add(new HitDetectionSystem(contexts, physicsRaycaster))
                    
                    //Все создания/пердвижения/удаления должны произойти до этой системы
                    .Add(new PhysicsSimulateSystem(physicsScene))
                    
                    .Add(new TransformSenderSystem(matchId, contexts, udpSendUtils))
                    
                    .Add(new InputClearSystem(contexts))
                    
                    
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
}