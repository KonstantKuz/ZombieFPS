using System;
using UnityEngine;

namespace App.Weapon.Projectile.Data
{
    [Serializable]
    public class ProjectileParams
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _hitRadius;
        [SerializeField]
        private float _maxDistance;
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        public float HitRadius
        {
            get => _hitRadius;
            set => _hitRadius = value;
        } 
        public float MaxDistance
        {
            get => _maxDistance;
            set => _maxDistance = value;
        }

    }
}