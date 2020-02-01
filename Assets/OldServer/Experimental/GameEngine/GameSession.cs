using System;
using AmoebaBattleServer01.Experimental.GameEngine.Systems;
using NetworkLibrary.NetworkLibrary.Http;

namespace AmoebaBattleServer01.Experimental.GameEngine
{
    /// <summary>
    ///  Хранит системы и информацию о игроках игровой сессии.
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

            CheckEmpty(Contexts);
            systems = new Entitas.Systems();
            systems.Add(new PlayersInitSystem(Contexts, roomData));
            systems.Add(new PlayerJoystickInputHandlerSystem(Contexts));
            systems.Add(new NetworkSenderSystem(Contexts));

            systems.Initialize();
            gameStartTime = DateTime.UtcNow;
        }

        
        private void CheckEmpty(Contexts contexts)
        {
            int countOfEntities = contexts.game.GetEntities().Length;
            countOfEntities += contexts.input.GetEntities().Length;
            if(countOfEntities!=0)
                throw new Exception("Контекст не пустой");
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
        public static readonly TimeSpan GameDuration = new TimeSpan(0,0,800);
    } 
}