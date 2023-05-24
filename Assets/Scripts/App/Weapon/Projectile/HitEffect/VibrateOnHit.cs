using App.Unit.Component.Attack;
using App.Vibration;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.HitEffect
{
    public class VibrateOnHit : ProjectileHitEffect
    {
        [SerializeField] private VibrationType _type;
        [Inject] private VibrationManager _manager;
        public override bool OnHit(HitInfo hitInfo)
        {
            _manager.Vibrate(_type);
            return true;
        }
    }
}