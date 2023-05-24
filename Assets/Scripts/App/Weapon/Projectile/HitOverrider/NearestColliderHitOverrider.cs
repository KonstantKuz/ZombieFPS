using System.Linq;
using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitOverrider
{
    public class NearestColliderHitOverrider : MonoBehaviour, IHitOverrider
    {
        [SerializeField] private Transform _collidersRoot;
       
        public HitInfo OverrideHit(HitInfo hitInfo)
        {
            return new HitInfo(GetClosestCollider(hitInfo.Position).gameObject, hitInfo.Position, hitInfo.Normal);
        }

        private Collider GetClosestCollider(Vector3 point)
        {
            return _collidersRoot.GetComponentsInChildren<Collider>(true)
                .OrderBy(it => Vector3.Distance(it.transform.position, point))
                .First();
        }
    }
}
