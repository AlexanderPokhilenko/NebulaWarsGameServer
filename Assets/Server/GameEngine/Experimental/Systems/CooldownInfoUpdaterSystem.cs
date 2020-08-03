using System.Collections.Generic;
using System.Linq;
using Entitas;
using Libraries.NetworkLibrary.Udp.ServerToPlayer.BattleStatus;
using Server.Udp.Sending;

namespace Server.GameEngine.Experimental.Systems
{
    public class CooldownInfoUpdaterSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly int matchId;
        private readonly UdpSendUtils udpSendUtils;
        private readonly GameContext gameContext;
        private readonly IGroup<GameEntity> playersGroup;
        private readonly Dictionary<int, WeaponInfo[]> lastWeaponInfos;
        
        public CooldownInfoUpdaterSystem(Contexts contexts, int matchId, UdpSendUtils udpSendUtils)
        {
            this.matchId = matchId;
            this.udpSendUtils = udpSendUtils;
            gameContext = contexts.game;
            playersGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Player).NoneOf(GameMatcher.Bot));
            lastWeaponInfos = new Dictionary<int, WeaponInfo[]>(10);
        }

        public void Initialize()
        {
            foreach (var player in playersGroup)
            {
                lastWeaponInfos.Add(player.player.id, new WeaponInfo[0]);
            }
        }

        public void Execute()
        {
            foreach (var gameEntity in playersGroup)
            {
                var playerId = gameEntity.player.id;

                var abilityCooldown = gameEntity.hasAbility ? gameEntity.ability.cooldown : 0f;

                WeaponInfo[] weaponInfos;
                if (gameEntity.hasSkin)
                {
                    var skin = gameEntity.skin.value;
                    weaponInfos = gameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
                        .Select(e => new WeaponInfo(skin.Apply(e.cannon.bullet.typeId), e.cannon.cooldown)).ToArray();
                }
                else
                {
                    weaponInfos = gameEntity.GetAllChildrenGameEntities(gameContext, c => c.hasCannon)
                        .Select(e => new WeaponInfo(e.cannon.bullet.typeId, e.cannon.cooldown)).ToArray();
                }

                if (weaponInfos.SequenceEqual(lastWeaponInfos[playerId])) continue;

                lastWeaponInfos[playerId] = weaponInfos;
                udpSendUtils.SendCooldownInfo(matchId, playerId, abilityCooldown, weaponInfos);
            }    
        }
    }
}