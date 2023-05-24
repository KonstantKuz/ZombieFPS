using System;
using UnityEngine;

namespace App.Unit.Component.Health
{
    public class OneHitDamageable : MonoBehaviour, IDamageable
    {
        public bool DamageEnabled { get; set; } = true;
        
        public event Action<DamageInfo> OnDamageTaken;
        
        public void TakeDamage(DamageInfo damage)
        {
            Destroy(gameObject);
        }
    }
}