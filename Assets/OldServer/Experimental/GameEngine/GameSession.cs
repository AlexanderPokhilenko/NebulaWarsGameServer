using System;
using NetworkLibrary.NetworkLibrary.Http;
using OldServer.Experimental.GameEngine.Systems;
using UnityEngine;

namespace OldServer.Experimental.GameEngine
{
    /// <summary>
    ///  Хранит системы и информацию о игроках.
    /// </summary>
    public class GameSession
    {
        private Entitas.Systems systems;
        public Contexts Contexts;
        public GameRoomData RoomData;
        private DateTime? gameStartTime;

        private readonly GameSessionsStorage gameSessionsStorage;

        public GameSession(GameSessionsStorage gameSessionsStorage)
        {
            this.gameSessionsStorage = gameSessionsStorage;
        }

        public void ConfigureSystems(GameRoomData roomData)
        {
            Debug.Log("Создание новой комнаты номер = "+roomData.GameRoomNumber);

            RoomData = roomData;
            Contexts = new Contexts();
            Contexts.SubscribeId();
#if UNITY_EDITOR
            CollidersDrawer.contextsList.Add(Contexts);
            Debug.Log("Количество контекстов: " + CollidersDrawer.contextsList.Count);
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

            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }
        
        public void Execute()
        {
            if (SessionTimedOut())
            {
                MarkGameAsFinished();
                return;
            }
            systems.Execute();
        }

        private bool SessionTimedOut()
        {
            if (gameStartTime != null)
            {
                TimeSpan gameDuration = DateTime.UtcNow - gameStartTime.Value;
                return gameDuration > GameSessionGlobals.GameDuration;
            }
            return false;
        }

        private void MarkGameAsFinished()
        {
            gameSessionsStorage.MarkGameAsFinished(RoomData.GameRoomNumber);
        }

        public void Cleanup()
        {
            systems.Cleanup();
        }
    }

    public static class GameSessionGlobals
    {
        public static readonly TimeSpan GameDuration = new TimeSpan(0,0,300);
    } 
}