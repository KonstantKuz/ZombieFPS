using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class AimTowardsBarrelModifier : ProjectileModifier
    {
        [SerializeField] private Transform _barrelDirection;
        
        public override void Modify(Projectiles.Projectile projectile)
        {
            projectile.transform.rotation = _barrelDirection.rotation;
        }
    }
}