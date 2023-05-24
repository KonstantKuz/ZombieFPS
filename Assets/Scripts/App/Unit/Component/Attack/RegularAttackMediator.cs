using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit.Component.Attack.Animation;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;
using App.Unit.Component.ComponentProvider;
using App.Unit.Component.Health;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine.Profiling;

namespace App.Unit.Component.Attack
{
    public class RegularAttackMediator : IAttackMediator
    {
        private IComponentProvider<IAttackComponent> _componentProvider;
        public ICollection<IAttackComponent> Components => _componentProvider.Components;
        public bool CanAttack => Get<IAttackCondition>().CanStartAttack;
        public bool IsAttacking { get; private set; }
        public event Action OnFire; 
        public event Action OnDamaged;
        
        public T Get<T>() where T : IAttackComponent => _componentProvider.Get<T>();

        public void SetProvider(IComponentProvider<IAttackComponent> componentProvider)
        {
            _componentProvider = componentProvider;
            Get<IAttackAnimation>().OnFire += FireImmediately;  
            Get<IAttackAnimation>().OnFireAnimationFinished += OnAnimationFinished;
        }

        public void Attack()
        {
            if (!Get<IAttackCondition>().CanStartAttack) {
                this.Logger().Error("Can not start attack");
                return;
            }

            IsAttacking = true;
            Get<IAttackAnimation>().Play();
        }
        
        private void FireImmediately()
        {
            if (!Get<IAttackCondition>().CanFireImmediately) return;
            
            Profiler.BeginSample("RegularAttackMediator.FireImmediately");
            Get<IWeaponWrapper>().Fire();
            OnFire?.Invoke();
            Profiler.EndSample();
        }
        private void OnAnimationFinished()
        {
            IsAttacking = false;
            Get<AttackIntervalTimer>().StartTimer(null);
        }
        public void Damage(HitInfo hitInfo)
        {
            if (hitInfo.Target.GetComponentInParent<IDamageable>() == null) return;
            Get<IDamager>().Damage(hitInfo);
            OnDamaged?.Invoke();
        }
        public void Dispose()
        {
            Get<IAttackAnimation>().OnFire -= FireImmediately;
            Get<IAttackAnimation>().OnFireAnimationFinished -= OnAnimationFinished;
            Components.OfType<IDisposable>().ForEach(it=>it.Dispose());
        }
    }
}