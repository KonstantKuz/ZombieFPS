using System;
using App.Animation;
using App.Unit.Component.Animation;
using App.Unit.Component.Attack.Condition;
using Feofun.Components;
using Feofun.Extension;
using Feofun.Util.Animator;
using UniRx;
using UnityEngine;

namespace App.Unit.Component.Attack.Animation
{
    public class RegularAttackAnimation : AttackComponent, IInitializable<AttackComponentInitData>, IDisposable, IAttackAnimation, IAttackCondition
    {
        protected static readonly int _attackStateHash = Animator.StringToHash("Attack");
        protected static readonly int _attackHash = Animator.StringToHash("Attack");

        protected static readonly int _interruptionStateHash = Animator.StringToHash("AttackInterruption");
        protected static readonly int _interruptHash = Animator.StringToHash("InterruptAttack");

        private AnimationEventHandler _animationEventHandler;
        protected AnimatorTween _animatorTween;
        protected bool _startedAnimation;
        protected CompositeDisposable _animationDisposable;

        public bool CanStartAttack => !_startedAnimation;
        public bool CanFireImmediately => _startedAnimation;
        
        public event Action OnFire;
        public event Action OnFireAnimationFinished;
        

        public void Init(AttackComponentInitData data)
        {
            _animationEventHandler = data.AttackRoot.gameObject.RequireComponentInChildren<AnimationEventHandler>();
            _animatorTween = AnimatorTween.Create(_animationEventHandler.gameObject);
            _animationEventHandler.OnEvent += OnAnimationCallback;
        }

        public virtual void Play()
        {
            DisposeAnimation();
            _animationDisposable = new CompositeDisposable();
            _startedAnimation = true;
            SetAnimationParam();
        }

        protected virtual void SetAnimationParam()
        {
            _animatorTween.WaitForEventFromTrigger(_attackHash, _attackStateHash, OnAttackAnimationFinished)
                .AddTo(_animationDisposable);
        }

        protected virtual void OnAttackAnimationFinished()
        {
            DisposeAnimation();
            InvokeOnAnimationFinished();
            _startedAnimation = false;
        }

        public virtual void Interrupt()
        {
            _animatorTween.WaitForEventFromTrigger(_interruptHash, _interruptionStateHash, OnAttackAnimationFinished)
                .AddTo(_animationDisposable);
        }

        protected void InvokeOnAnimationFinished()
        {
            OnFireAnimationFinished?.Invoke();
        }

        protected virtual void OnAnimationCallback(string eventName)
        {
            if (eventName.Equals(AnimationEvents.FIRE)) {
                OnFire?.Invoke();
            }
        }

        private void DisposeAnimation() => _animationDisposable?.Dispose();

        public virtual void Dispose()
        {
            DisposeAnimation();
            _animationEventHandler.OnEvent -= OnAnimationCallback;
        }
    }
}