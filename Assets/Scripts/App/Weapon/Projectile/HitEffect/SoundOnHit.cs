using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public class SoundOnHit : ProjectileHitEffect
    {
        [SerializeField] private AudioClip _audioClip;

        public override bool OnHit(HitInfo hitInfo)
        {
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            return true;
        }
    }
}