using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.Utils;

namespace Server.GameEngine
{
    public class Battle
    {
        public Contexts Contexts;
        public GameRoomData RoomData;
        
        private Entitas.Systems systems;
        private readonly BattlesStorage battlesStorage;
        private CancellationTokenSource delayedStopTokenSource;
        public Battle(BattlesStorage battlesStorage)
        {
            this.battlesStorage = battlesStorage;
        }

        public void Start(GameRoomData roomData)
        {
            Log.Info("Создание новой комнаты номер = "+roomData.GameRoomNumber);

            RoomData = roomData;
            Contexts = new Contexts();
            Contexts.SubscribeId();
            
            #if UNITY_EDITOR
                CollidersDrawer.contextsList.Add(Contexts);
                Log.Info("Количество контекстов: " + CollidersDrawer.contextsList.Count);
            #endif

            systems = new Entitas.Systems()
                .Add(new PlayersInitSystem(Contexts, roomData))
                .Add(new PlayerMovementHandlerSystem(Contexts))
                .Add(new PlayerAttackHandlerSystem(Contexts))
                .Add(new ParentsSystems(Contexts))
                .Add(new MovementSystems(Contexts))
                .Add(new GlobalTransformSystem(Contexts)) // Обернуть в Feature?
                .Add(new ShootingSystems(Contexts))
                .Add(new CollisionSystems(Contexts))
                .Add(new EffectsSystems(Contexts))
                .Add(new TimeSystems(Contexts))
                .Add(new DestroySystems(Contexts))
                .Add(new AISystems(Contexts))
                .Add(new NetworkSenderSystem(Contexts))
                .Add(new InputDeletingSystem(Contexts))
                .Add(new FinishBattleSystem(Contexts, this))
                ;

            systems.Initialize();

            
            delayedStopTokenSource = new CancellationTokenSource();
            var token = delayedStopTokenSource.Token;
            
#pragma warning disable 4014
            FinishBattleAfterDelayAsync(GameSessionGlobals.GameDuration, token);
#pragma warning restore 4014
        }
        
        private async Task FinishBattleAfterDelayAsync(TimeSpan delay, CancellationToken token)
        {
            await Task.Delay(delay, token);
            StopTicks();
        }
        
        public void Execute()
        {
            systems.Execute();
        }

        public void Cleanup()
        {
            systems.Cleanup();
        }

        public void StopTicks()
        {
            Log.Error("Остановка боя.");
            delayedStopTokenSource.Cancel();
            battlesStorage.MarkBattleAsFinished(RoomData.GameRoomNumber);
        }
    }
}