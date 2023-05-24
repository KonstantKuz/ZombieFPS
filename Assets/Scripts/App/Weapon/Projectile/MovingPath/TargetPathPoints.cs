using System.Collections;
using System.Collections.Generic;
using App.Unit.Component.Target;
using UnityEngine;

namespace App.Weapon.Projectile.MovingPath
{
    public class TargetPathPoints : IEnumerator<Vector3>
    {
        private readonly IEnumerator<ITarget> _targetsEnumerator;

        private Vector3 _lastTargetPosition;

        public Vector3 Current
        {
            get
            {
                if (!_targetsEnumerator.Current.IsValid()) {
                    return _lastTargetPosition;
                }
                return _lastTargetPosition = _targetsEnumerator.Current.Center.position;
            }
        }
        object IEnumerator.Current => Current;
        public TargetPathPoints(IEnumerable<ITarget> targets)
        {
            _targetsEnumerator = targets.GetEnumerator();
        
        }
        public bool MoveNext() => _targetsEnumerator.MoveNext(); 

        public void Reset() { }
        
        public void Dispose() => _targetsEnumerator.Dispose();
    }
}