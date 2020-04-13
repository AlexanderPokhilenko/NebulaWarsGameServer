using System.Linq;
using Entitas;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using Server.Udp.Sending;

namespace Server.GameEngine.Systems
{
    //TODO отправлять сообщения только при изменении
    public class CooldownInfoUpdaterSystem : IExecuteSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> playersGroup;
        
        public CooldownInfoUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            playersGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
        }

        public void Execute()
        {
            foreach (var gameEntity in playersGroup)
            {
                var playerId = gameEntity.player.id;

                var abilityCooldown = gameEntity.hasAbility ? gameEntity.ability.cooldown : 0f;
                
                var weaponInfos = gameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
                    .Select(e => new WeaponInfo(e.cannon.bullet.typeId, e.cannon.cooldown)).ToArray();

                udpSendUtils.SendCooldownInfo(matchId, playerId, abilityCooldown, weaponInfos);
            }    
        }
    }
}