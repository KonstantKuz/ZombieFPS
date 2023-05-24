using System;

namespace App.Unit.Component.Attack.Animation
{
    public interface IAttackAnimation : IAttackComponent
    {
        event Action OnFire;       
        event Action OnFireAnimationFinished;
        void Play();
        void Interrupt();
    }
}