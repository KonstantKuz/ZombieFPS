using System;
using App.Unit.Component.Attack.Animation;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;

namespace App.Unit.Component.Attack.Builder
{
    public class AttackBuilder
    {
        private readonly IComponentBuilder<IAttackComponent> _componentBuilder;
        private IAttackMediator _attackMediator;

        public AttackBuilder(AttackComponentInitData initData)
        {
            _componentBuilder = new AttackComponentBuilder(initData);
        }

        public static AttackBuilder Create(AttackComponentInitData initData) => new(initData);

        public AttackBuilder SetMediator(IAttackMediator attackMediator)
        {
            _attackMediator = attackMediator;
            return this;
        }

        public AttackBuilder Register(AttackComponent component)
        {
            component.SetMediator(_attackMediator);
            _componentBuilder.Register(component);
            return this;
        }

        public AttackBuilder Register<T>(AttackComponent component) where T : IAttackComponent
        {
            component.SetMediator(_attackMediator);
            _componentBuilder.Register<T>(component);
            return this;
        }

        public IAttackMediator BuildRegular()
        {
            return Build(
                typeof(IAttackCondition),
                typeof(IAttackAnimation),
                typeof(IWeaponWrapper),
                typeof(AttackIntervalTimer),
                typeof(IDamager));
        }
        
        private IAttackMediator Build(params Type[] requiredTypes)
        {
            var components = _componentBuilder.Build(requiredTypes);
            _attackMediator.SetProvider(components);
            return _attackMediator;
        }
    }
}