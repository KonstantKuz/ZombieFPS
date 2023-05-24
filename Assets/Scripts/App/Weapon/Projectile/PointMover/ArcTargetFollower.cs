using Feofun.Extension;
using UnityEngine;

namespace App.Weapon.Projectile.PointMover
{
    public class ArcTargetFollower : ITargetFollower
    {
        private readonly Vector3 _startPoint;
        private readonly Vector3 _arcDirection;
        private readonly float _arcWidth;
        private readonly float _speed;
        private readonly bool _rotateAlongDirection;

        private float _moveTime;

        public ArcTargetFollower(Vector3 startPoint, Vector3 arcDirection, float arcWidth, float speed, bool rotateAlongDirection)
        {
            _moveTime = 0;
            _arcDirection = arcDirection;
            _startPoint = startPoint;
            _arcWidth = arcWidth;
            _speed = speed;
            _rotateAlongDirection = rotateAlongDirection;
        }

        public void OnTick(Transform own, Vector3 target)
        {
            _moveTime = IncreaseMovingTime(_moveTime);
            
            var moveDirection = target - own.position;

            if(_rotateAlongDirection && moveDirection != Vector3.zero) {
                own.rotation = Quaternion.LookRotation(moveDirection.XZ());
            }
            
            own.position = Vector3.Lerp(_startPoint, target, _moveTime);
            var arcLocalDirection = (own.rotation * _arcDirection).normalized;
            var acrOffset = arcLocalDirection * _arcWidth  * Mathf.Sin(_moveTime * Mathf.PI);
            own.position += acrOffset;
        }
        
        private float IncreaseMovingTime(float moveTime) => Mathf.Clamp01(moveTime + 1.0f * Time.fixedDeltaTime * _speed);
    }
}