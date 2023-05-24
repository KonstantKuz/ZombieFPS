using System;
using EasyButtons;
using UniRx;
using UnityEngine;

namespace App.Unit.Component.Health
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private bool _damageEnabled = true;
        
        private ReactiveProperty<float> _current;

        public event Action OnZeroHealth; 
        public event Action<DamageInfo> OnDamageTaken;
        
        public bool DamageEnabled 
        {
            get => _damageEnabled;
            set => _damageEnabled = value;
        } 
        public bool IsAlive => _current.Value > 0;
        public IReadOnlyReactiveProperty<float> Current => _current;
        
        public void Init(float maxValue)
        {
            _current = new ReactiveProperty<float>(maxValue);
        }
        [Button]
        public virtual void TakeDamage(DamageInfo damage)
        {
            if (!DamageEnabled) return;
            if(!IsAlive) return;
            
            ChangeHealth(-damage.Damage);
            OnDamageTaken?.Invoke(damage);
            if (!IsAlive) {
                OnZeroHealth?.Invoke();
            }
        }
        [Button]
        public void Kill()
        {
            if (!IsAlive) {
                return;
            }
            ChangeHealth(-Current.Value);
            OnZeroHealth?.Invoke();
        }
        protected void SetHealth(float value)
        {
            _current.Value = Mathf.Max(0, value);
        }
        private void ChangeHealth(float delta)
        {
            var newValue = Mathf.Max(0, _current.Value + delta);
            _current.Value = newValue;
        }
        
    }
}