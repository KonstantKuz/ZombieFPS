using System;
using App.Unit.Component.Health;

namespace App.Unit.Component
{
    public class DamageAmountTrigger : IDisposable
    {
        private readonly IDamageable _damageable;
        
        private float _observedDamageAmount;
        private float _currentDamageAmount;
        private Action _action;
        
        public DamageAmountTrigger(IDamageable damageable)
        {
            _damageable = damageable;
        }

        public static IDisposable SubscribeOnDamageAmountTaken(IDamageable damageable, float damageAmount, Action action)
        {
            return new DamageAmountTrigger(damageable).SubscribeOnDamageAmountTaken(damageAmount, action);
        }

        public IDisposable SubscribeOnDamageAmountTaken(float damageAmount, Action action)
        {
            _damageable.OnDamageTaken += OnDamageTaken;
            _observedDamageAmount = damageAmount;
            _currentDamageAmount = 0;
            _action = action;
            return this;
        }
        private void OnDamageTaken(DamageInfo damage)
        {
            _currentDamageAmount += damage.Damage;

            if (_currentDamageAmount >= _observedDamageAmount) {
                _action?.Invoke();
                _currentDamageAmount = 0;
            }
        }
        public void Dispose()
        {
            _damageable.OnDamageTaken -= OnDamageTaken;
            _action = null;
        }

    }
}