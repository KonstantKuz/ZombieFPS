using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class InitialPlaceModifier : ProjectileModifier
    {
        [SerializeField] private Transform _projectilePosition;
        protected virtual Transform ProjectilePosition => _projectilePosition;
        
        public override void Modify(Projectiles.Projectile projectile)
        {
            projectile.transform.position = ProjectilePosition.position;
        }
    }
}