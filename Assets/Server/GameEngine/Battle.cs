using System;
using NetworkLibrary.NetworkLibrary.Http;
using Server.GameEngine.Systems;
using Server.Utils;
using UnityEditor;
using UnityEngine;

namespace Server.GameEngine
{
    public class Battle
    {
        private Entitas.Systems systems;
        public Contexts Contexts { get; private set; }
        public GameRoomData RoomData { get; private set; }
        private DateTime? gameStartTime;

        private readonly BattlesStorage gameSessionsStorage;

        private readonly FlameCircleObject zoneObject;

        private bool GameOver;
        public Battle(BattlesStorage gameSessionsStorage)
        {
            this.gameSessionsStorage = gameSessionsStorage;
            //TODO: как-то обойтись без использования AssetDatabase; добавить возможность менять параметры зоны для разных карт
            zoneObject = AssetDatabase.LoadAssetAtPath<FlameCircleObject>("Assets/SO/BaseObjects/FlameCircle.asset");
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
                    .Add(new ZoneInitSystem(Contexts, zoneObject))
                    .Add(new PlayersInitSystem(Contexts, roomData))
                    .Add(new AsteroidsInitSystem(Contexts))
                    .Add(new PlayerMovementHandlerSystem(Contexts))
                    .Add(new PlayerAttackHandlerSystem(Contexts))
                    .Add(new ParentsSystems(Contexts))
                    .Add(new AISystems(Contexts))
                    .Add(new MovementSystems(Contexts))
                    .Add(new GlobalTransformSystem(Contexts))
                    .Add(new ShootingSystems(Contexts))
                    .Add(new CollisionSystems(Contexts))
                    .Add(new EffectsSystems(Contexts))
                    .Add(new TimeSystems(Contexts))
                    .Add(new DestroySystems(Contexts))
                    .Add(new NetworkSenderSystem(Contexts))
                    .Add(new InputDeletingSystem(Contexts))
                    .Add(new FinishBattleSystem(Contexts, this))
                ;

            systems.ActivateReactiveSystems();
            systems.Initialize();
            //Contexts.game
            var entity = AssetDatabase.LoadAssetAtPath<BaseWithHealthObject>("Assets/SO/BaseObjects/SpaceStation.asset")
                .CreateEntity(Contexts.game);
            entity.AddPosition(Vector2.zero);
            entity.AddDirection(0);
            gameStartTime = DateTime.UtcNow;
        }
        
        public void Execute()
        {
            if(GameOver) return;
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

        public void FinishGame()
        {
            GameOver = true;
            gameSessionsStorage.MarkBattleAsFinished(RoomData.GameRoomNumber);
            systems.DeactivateReactiveSystems();
            systems.TearDown();
            systems.ClearReactiveSystems();
        }
    }
}