using System.Collections.Generic;
using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers.MultiShoot
{
    public class FixedAngleMultiShootModifier : MultiShootModifier
    {
        [SerializeField] private float _angleStep;
        
        public override void Modify(List<Projectiles.Projectile> projectiles)
        {
            var fullAngle = projectiles.Count * _angleStep;
            var rotation = Quaternion.Euler(0, -fullAngle / 2, 0);
            foreach (var projectile in projectiles)
            {
                projectile.transform.localRotation *= rotation;
                rotation *= Quaternion.Euler(0, _angleStep, 0);
            }
        }
    }
}