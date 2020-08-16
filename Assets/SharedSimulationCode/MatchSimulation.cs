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
                    
                    //Отправка текущего состояния мира
                    .Add(new PlayersSendingSystem(matchId, contexts, udpSendUtils))
                    .Add(new TransformSenderSystem(matchId, contexts, udpSendUtils))
                    .Add(new HealthSenderSystem(contexts, matchId, udpSendUtils))
                    .Add(new MaxHealthSenderSystem(contexts, matchId, udpSendUtils))
                    
                    //Очистка
                    .Add(new InputClearSystem(contexts))
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
}