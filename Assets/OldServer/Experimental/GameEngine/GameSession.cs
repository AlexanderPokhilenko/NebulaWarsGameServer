using System;
using AmoebaBattleServer01.Experimental.GameEngine.Systems;
using NetworkLibrary.NetworkLibrary.Http;

namespace AmoebaBattleServer01.Experimental.GameEngine
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
            Console.WriteLine("Создание новой комнаты номер = "+roomData.GameRoomNumber);

            RoomData = roomData;
            Contexts = new Contexts();

            systems = new Entitas.Systems()
                .Add(new PlayersInitSystem(Contexts, roomData))
                .Add(new PlayerMovementHandlerSystem(Contexts))
                .Add(new PlayerAttackHandlerSystem(Contexts))
                .Add(new NetworkSenderSystem(Contexts));

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