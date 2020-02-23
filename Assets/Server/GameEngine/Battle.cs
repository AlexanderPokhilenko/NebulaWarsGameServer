using System;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.Utils;

namespace Server.GameEngine
{
    public class Battle
    {
        private Entitas.Systems systems;
        public Contexts Contexts { get; private set; }
        public GameRoomData RoomData { get; private set; }
        private DateTime? gameStartTime;

        private readonly BattlesStorage gameSessionsStorage;

        public Battle(BattlesStorage gameSessionsStorage)
        {
            this.gameSessionsStorage = gameSessionsStorage;
        }

        public void ConfigureSystems(GameRoomData roomData)
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
                .Add(new InputDeletingSystem(Contexts));

            systems.ActivateReactiveSystems();
            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }
        
        public void Execute()
        {
            if (IsSessionTimedOut())
            {
                FinishGame();
                return;
            }
            systems.Execute();
        }

        public void Cleanup()
        {
            systems.Cleanup();
        }

        private bool IsSessionTimedOut()
        {
            if (gameStartTime == null) return false;
            var gameDuration = DateTime.UtcNow - gameStartTime.Value;
            return gameDuration > GameSessionGlobals.GameDuration;
        }

        private void FinishGame()
        {
            gameSessionsStorage.MarkGameAsFinished(RoomData.GameRoomNumber);
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
        }
    }

    public static class GameSessionGlobals
    {
        public static readonly TimeSpan GameDuration = new TimeSpan(0,0,900);
    } 
}