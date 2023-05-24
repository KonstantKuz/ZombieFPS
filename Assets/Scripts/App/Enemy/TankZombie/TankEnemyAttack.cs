using App.Animation;
using App.Enemy.Component.Attack;
using App.Unit.Component.Animation;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Builder;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;
using Feofun.Extension;

namespace App.Enemy.TankZombie
{
    public class TankEnemyAttack : BaseAttack
    {
        private TankZombieWeapon _weapon;
        private AnimationEventHandler _animationEventHandler;
        private EnemyAttackAnimation _attackAnimation;

        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<TankZombieWeapon>();
            _animationEventHandler = gameObject.RequireComponentInChildren<AnimationEventHandler>();
        }

        public override void Init(Unit.Unit owner)
        {
            base.Init(owner);
            _animationEventHandler.OnEvent += OnAnimationEvent;
        }

        protected override IAttackMediator BuildAttack(Unit.Unit owner)
        {
            _attackAnimation = new EnemyAttackAnimation();
            
            return AttackBuilder.Create(new AttackComponentInitData(owner, owner.transform))
                .SetMediator(new RegularAttackMediator())
                .Register(_attackAnimation)
                .Register(new RegularAttackCondition())
                .Register(new AttackIntervalTimer(owner.Model.AttackModel.AttackInterval))
                .Register(new WeaponWrapper())
                .Register(new RegularDamager())
                .BuildRegular();
        }

        public override void DisposeLastAttack()
        {
            _attackAnimation.DisposeLastLaunchedAnimation();
            _weapon.Clear();
        }

        private void OnAnimationEvent(string eventName)
        {
            if(eventName != AnimationEvents.SHOW_PROJECTILE) return;

            _weapon.PrepareProjectile(InterruptAttack);
        }

        private void InterruptAttack()
        {
            _attackAnimation.Interrupt();
            _weapon.Clear();
        }

        protected override void Dispose()
        {
            base.Dispose();
            _weapon.Clear();
            if(_animationEventHandler == null) return;
            _animationEventHandler.OnEvent -= OnAnimationEvent;
        }


    }
}