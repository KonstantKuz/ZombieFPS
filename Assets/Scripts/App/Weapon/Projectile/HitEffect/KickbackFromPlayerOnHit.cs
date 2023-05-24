using App.Player.Service;
using App.Unit.Component.Attack;
using App.Unit.Component.DamageReaction;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.HitEffect
{
    public class KickbackFromPlayerOnHit : ProjectileHitEffect
    {
        [SerializeField] private KickbackReactionParams _kickbackParams;

        [Inject] private PlayerService _playerService;
        
        public override bool OnHit(HitInfo hitInfo)
        {
            if (_playerService.Player == null) return false;
            
            var direction = hitInfo.Target.transform.position - _playerService.Player.transform.position;
            KickbackReaction.TryExecuteOn(hitInfo.Target, direction, _kickbackParams);
            return true;
        }
    }
}