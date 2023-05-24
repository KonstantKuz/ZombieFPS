using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class AccuracyModifier : ProjectileModifier
    {
        [SerializeField] private float _maxSpreadAngle;
        
        public override void Modify(Projectiles.Projectile projectile)
        {
            projectile.transform.localRotation *= Quaternion.Euler(GetRandomSpreadAngle(), GetRandomSpreadAngle(), 0);
        }

        protected virtual float GetRandomSpreadAngle()
        {
            return Random.Range(-_maxSpreadAngle, _maxSpreadAngle);
        }
    }
}