using App.Unit.Component.Attack.Animation;
using UniRx;

namespace App.Enemy.Component.Attack
{
    public class EnemyAttackAnimation : RegularAttackAnimation
    {
        protected override void SetAnimationParam()
        {
            _animatorTween.WaitForEventFromBool(_attackHash, true, _attackStateHash, OnAttackAnimationFinished)
                .AddTo(_animationDisposable);
        }

        protected override void OnAttackAnimationFinished()
        {
            _animatorTween.SetBool(_attackHash, false);
            base.OnAttackAnimationFinished();
        }
        
        public void DisposeLastLaunchedAnimation()
        {
            OnAttackAnimationFinished();
        }
    }
}