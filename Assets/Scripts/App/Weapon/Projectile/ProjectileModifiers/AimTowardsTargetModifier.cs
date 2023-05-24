using UnityEngine;
using UnityEngine.Assertions;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class AimTowardsTargetModifier : ProjectileModifier
    {
        public override void Modify(Projectiles.Projectile projectile)
        {
            Assert.IsTrue(projectile.Target != null);
            var targetPosition = projectile.Target.Center.position; 
            projectile.transform.rotation = Quaternion.LookRotation(targetPosition - projectile.transform.position);
        }
    }
}