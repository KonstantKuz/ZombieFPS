using App.UI.Screen.World.Player.Crosshair;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class AimTowardsCrosshairModifier : ProjectileModifier
    {
        [SerializeField] private float _minHitRayDistance = 20f;
        [SerializeField] private float _maxHitRayDistance = 100f;
       
        [Inject] private CrosshairRaycaster _crosshair;

        public override void Modify(Projectiles.Projectile projectile)
        {
            projectile.transform.rotation = GetRotationToCrosshairHit(projectile.transform.position);
        }

        private Quaternion GetRotationToCrosshairHit(Vector3 projectilePosition)
        {
            return Quaternion.LookRotation(GetHitPoint(projectilePosition) - projectilePosition);
        }

        private Vector3 GetHitPoint(Vector3 projectilePosition)
        {
            var hitRay = _crosshair.HitRay;
            if (!Physics.Raycast(hitRay, out RaycastHit hit))
            {
                return hitRay.GetPoint(_maxHitRayDistance);
            }

            var distanceToHit = Vector3.Distance(projectilePosition, hit.point);
            return distanceToHit > _minHitRayDistance ? hit.point : hitRay.GetPoint(_minHitRayDistance);
        }
    }
}