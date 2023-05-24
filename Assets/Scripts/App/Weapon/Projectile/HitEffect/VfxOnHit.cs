using App.Unit.Component.Attack;
using App.Unit.Component.Layering;
using Feofun.Util;
using Feofun.World.Factory.ObjectFactory.Factories;
using Feofun.World.Model;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.HitEffect
{
    public class VfxOnHit : ProjectileHitEffect
    {
        [SerializeField] private WorldObject _vfxOnEnemyHit;
        [SerializeField] private WorldObject _vfxOnGeometryHit;

        [Inject] private ObjectPoolFactory _objectFactory;

        public override bool OnHit(HitInfo hitInfo)
        {
            var vfxPrefab = GetVfxByTarget(hitInfo.Target);
            if (vfxPrefab == null) {
                IsExecuted = true;
                return false;
            }
            var hitVfx = _objectFactory.Create<WorldObject>(vfxPrefab.ObjectId);
            hitVfx.transform.SetPositionAndRotation(hitInfo.Position,  VectorExt.OrientationByNormal(hitInfo.Normal));
            return true;
        }

        private WorldObject GetVfxByTarget(GameObject target)
        {
            return target.layer == LayerNames.ENEMY_LAYER_ID ? _vfxOnEnemyHit : _vfxOnGeometryHit;
        }
    }
}