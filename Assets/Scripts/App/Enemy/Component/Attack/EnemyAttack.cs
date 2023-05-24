using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Builder;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;

namespace App.Enemy.Component.Attack
{
    public class EnemyAttack : BaseAttack
    {
        private EnemyAttackAnimation _attackAnimation;
        
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
        
        public override void DisposeLastAttack() => _attackAnimation.DisposeLastLaunchedAnimation();

    }
}