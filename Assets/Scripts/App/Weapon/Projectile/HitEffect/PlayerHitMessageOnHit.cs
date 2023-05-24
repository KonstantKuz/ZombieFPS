using App.MainCamera.Config;
using App.Player.Messages;
using App.Unit.Component.Attack;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.HitEffect
{
    public class PlayerHitMessageOnHit : ProjectileHitEffect
    {
        [SerializeField] private ShakeEventType _eventType;
        
        [Inject] private IMessenger _messenger;
        
        public override bool OnHit(HitInfo hitInfo)
        {
            _messenger.Publish(new PlayerProjectileHitMessage { ShakeEventType = _eventType });
            return true;
        }
    }
}