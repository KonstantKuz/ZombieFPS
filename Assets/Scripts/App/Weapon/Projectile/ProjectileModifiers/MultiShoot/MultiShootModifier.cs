using System.Collections.Generic;
using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers.MultiShoot
{
    public abstract class MultiShootModifier : MonoBehaviour
    {
        public abstract void Modify(List<Projectiles.Projectile> projectiles);
    }
}