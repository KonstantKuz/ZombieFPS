using System;
using App.Config;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Builder;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;
using App.Unit.Component.Death;
using App.Unit.Extension;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using JetBrains.Annotations;

using Zenject;

namespace App.Enemy.PoisonZombie.Attack
{
    public class PoisonZombieAttack : BaseAttack, IMessageListener<UnitDeathComponentMessage>
    {
        private PoisonZombieWeapon _weapon;
        private PoisonZombieAttackAnimation _animation;
        private Unit.Unit _owner;

        [CanBeNull]
        private IDisposable _damageTriggerDisposable;

        [Inject]
        private ConstantsConfig _constantsConfig;

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<PoisonZombieWeapon>();
            _weapon.Init(_constantsConfig.PoisonZombieAttackDuration);
        }

        public override void Init(Unit.Unit owner)
        {
            base.Init(owner);
            _owner = owner;
            _attack.OnFire += OnFire;
            _animation.OnFireAnimationFinished += OnFireAnimationFinished;

        }
        protected override IAttackMediator BuildAttack(Unit.Unit owner)
        {
            _animation = new PoisonZombieAttackAnimation(_constantsConfig.PoisonZombieAttackDuration);
            return  AttackBuilder.Create(new AttackComponentInitData(owner, owner.transform))
                .SetMediator(new RegularAttackMediator())
                .Register(_animation)
                .Register(new RegularAttackCondition())
                .Register(new AttackIntervalTimer(owner.Model.AttackModel.AttackInterval))
                .Register(new WeaponWrapper())
                .Register(new RegularDamager())
                .BuildRegular();
        }

        public override void DisposeLastAttack() => BreakAttack();

        private void OnFire()
        {
            _damageTriggerDisposable = 
                _owner.SubscribeOnDamageAmountTaken(_constantsConfig.PoisonZombieDamageAmountToBreakingAttack,
                    BreakAttack);
        }

        private void BreakAttack()
        {
            DisposeDamageTrigger();
            _animation.Interrupt();
        }

        private void OnFireAnimationFinished()
        {
            DisposeDamageTrigger();
            _weapon.FinishFire();
        }
        
        public void OnMessage(UnitDeathComponentMessage msg)
        {
            OnFireAnimationFinished();
            Dispose();
        }

        private void DisposeDamageTrigger()
        {
            _damageTriggerDisposable?.Dispose();
            _damageTriggerDisposable = null;
        }

        protected override void Dispose()
        {
            if (_attack != null) {
                _attack.OnFire -= OnFire;
            }
            if (_animation != null) {
                _animation.OnFireAnimationFinished -= OnFireAnimationFinished;
            }
            DisposeDamageTrigger();
            base.Dispose();
        }

    }
}