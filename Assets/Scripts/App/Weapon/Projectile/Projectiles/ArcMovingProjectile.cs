using App.Weapon.Projectile.PointMover;
using UnityEngine;

namespace App.Weapon.Projectile.Projectiles
{
    public class ArcMovingProjectile : Projectile
    {
        [SerializeField] private float _arcWidth;
        [SerializeField] private Vector3 _arcDirection;
        [SerializeField] private bool _rotateAlongDirection = true; 
        [SerializeField] private bool _followToTarget = true; 
        [SerializeField] private bool _getTargetCenter = true;

        private Vector3 _targetPosition;
        private ArcTargetFollower _targetFollower;
        
        public override void Launch()
        {
            base.Launch();
            _targetPosition = GetTargetPosition();
            _targetFollower = new ArcTargetFollower(transform.position, _arcDirection, _arcWidth, Speed, _rotateAlongDirection);
        }

        private void FixedUpdate()
        {
            if (_followToTarget) {
                _targetPosition = GetTargetPosition();
            }
            _targetFollower?.OnTick(transform, _targetPosition);
        }

        private Vector3 GetTargetPosition()
        {
            if (Target == null) {
                return _targetPosition;
            }

            return _getTargetCenter ? Target.Center.position : Target.Root.position;
        }

        private void OnDisable()
        {
            _targetFollower = null;
        }
    }
}