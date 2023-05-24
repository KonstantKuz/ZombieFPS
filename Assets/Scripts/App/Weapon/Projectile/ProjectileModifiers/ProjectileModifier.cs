using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public abstract class ProjectileModifier : MonoBehaviour
    {
        public abstract void Modify(Projectiles.Projectile projectile);
    }
}