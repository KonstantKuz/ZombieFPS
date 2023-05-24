using System;

namespace App.Unit.Component.Health
{
    public interface IDamageable
    {
        public bool DamageEnabled { get; set; }
        event Action<DamageInfo> OnDamageTaken;
        void TakeDamage(DamageInfo damage);
    }
}