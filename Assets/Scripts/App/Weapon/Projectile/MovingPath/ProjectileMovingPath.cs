using System;
using System.Collections.Generic;
using App.Unit.Component.Target;
using UnityEngine;

namespace App.Weapon.Projectile.MovingPath
{
    public class ProjectileMovingPath
    {
        private const float STOPPING_DISTANCE = 0.5f;

        private readonly Transform _own;
        private readonly IEnumerator<Vector3> _pathPoints;
        private readonly Transform _returnPoint;
        private readonly MovingPathType _movingPathType;
        
        public bool IsReturnBack { get; private set; }
        public MovingPathType MovingPathType => _movingPathType;
        
        public Vector3 PreviousPoint { get; private set; }
        public Vector3 NextPoint => !IsReturnBack ? _pathPoints.Current : _returnPoint.position;

        public bool IsNextPointReached => CurrentDistanceToNextPoint < STOPPING_DISTANCE;
        public float CurrentDistanceToNextPoint => Vector3.Distance(_own.position, NextPoint);     
        public float DistanceToNextPoint => Vector3.Distance(PreviousPoint, NextPoint);
        
        public ProjectileMovingPath(Transform own, MovingPathType movingPathType, Transform returnPoint, IEnumerator<Vector3> pathPoints)
        {
            _own = own;
            _returnPoint = returnPoint;
            _pathPoints = pathPoints;
            _movingPathType = movingPathType;
        }

        public bool MoveNext()
        {
            PreviousPoint = _own.position;
            var isMoveNext = _pathPoints.MoveNext();
            if (isMoveNext) {
                return true;
            }
            if (!IsReturnBack) {
                return IsReturnBack = true;
            }
            return false;
        }

        public static IEnumerator<Vector3> CreatePathPoints(MovingPathType type, 
            Transform own,
            IEnumerable<ITarget> targets,
            float forwardDistance) 
        {
            return type switch
            {
                MovingPathType.ByTarget => new TargetPathPoints(targets),
                MovingPathType.Forward => GetForwardPointEnumerator(own, forwardDistance),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        private static IEnumerator<Vector3> GetForwardPointEnumerator(Transform own, float forwardDistance)
        {
            yield return own.position + own.transform.forward * forwardDistance;
        }
    }
}