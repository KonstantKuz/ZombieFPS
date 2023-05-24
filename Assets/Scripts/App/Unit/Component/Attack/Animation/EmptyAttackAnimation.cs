using System;

namespace App.Unit.Component.Attack.Animation
{
    public class EmptyAttackAnimation : AttackComponent, IAttackAnimation
    {
        public event Action OnFire;
        public event Action OnFireAnimationFinished;

        public void Play()
        {
            OnFire?.Invoke();
            OnFireAnimationFinished?.Invoke();
        }

        public void Interrupt()
        {
        }
    }
}