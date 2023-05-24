using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile.Data;
using App.Weapon.Projectile.ProjectileModifiers;
using App.Weapon.Projectile.ProjectileModifiers.MultiShoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Weapon.Weapons
{
    [RequireComponent(typeof(InitialPlaceModifier))]
    public class MultiShootWeapon : RangedWeapon
    {
        [SerializeField] private List<MultiShootModifier> _groupModifiers;
        
        private int _shotCount;
        
        public override bool IsWeaponReady => true;
        
        public void Init(int shotCount)
        {
            _shotCount = shotCount;
        }
        
        protected override void CreateAndLaunchProjectile<T>(ITarget target, Action<HitInfo> hitCallback, 
            ProjectileParams projectileParams)
        {
            Assert.IsTrue(projectileParams != null);
            var projectiles = new List<Projectile.Projectiles.Projectile>();
            for (int i = 0; i < _shotCount; i++)
            {
                projectiles.Add(CreateProjectile<Projectile.Projectiles.Projectile>());
            }
            projectiles.ForEach(it => it.Init(target, hitCallback, projectileParams));
            projectiles.ForEach(ApplyModifiers);
            _groupModifiers.ForEach(it => it.Modify(projectiles));
            projectiles.ForEach(it => it.Launch());
        }
    }
}