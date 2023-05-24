using App.Unit.Component.Attack;
using Feofun.Extension;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    [CreateAssetMenu(fileName = "ProjectileHitFilter", menuName = "ScriptableObjects/ProjectileHitFilter")]
    public class ProjectileHitFilter : ScriptableObject
    {
        [SerializeField] 
        private LayerMask _filterMask;
        public bool Allow(HitInfo hitInfo)
        {
            return _filterMask.Contains(hitInfo.Target.layer);
        }
    }
}