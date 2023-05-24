using System;
using App.Unit.Component.Animation;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Animation;
using App.Unit.Component.Attack.Condition;
using Feofun.Components;
using Feofun.Extension;
using Feofun.Util.Animator;
using UniRx;
using UnityEngine;

namespace App.Enemy.PoisonZombie.Attack
{
    public class PoisonZombieAttackAnimation : AttackComponent, IInitializable<AttackComponentInitData>, IDisposable, IAttackAnimation, IAttackCondition
    {
        private static readonly int _startingAttackStateHash = Animator.StringToHash("StartingAttack");
        private static readonly int _finishingAttackStateHash = Animator.StringToHash("FinishingAttack");
        private static readonly int _attackHash = Animator.StringToHash("Attack");
 
        
        private readonly float _attackDuration;
        private float _attackInterval;
        
        private AnimatorTween _animatorTween;
        private bool _startedAnimation;
        private CompositeDisposable _disposable;
        private BlendShapeAnimation _blendShapeAnimation;

        private CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();

        public bool CanStartAttack => !_startedAnimation;
        public bool CanFireImmediately => _startedAnimation;

        public event Action OnFire;
        public event Action OnFireAnimationFinished;

        public PoisonZombieAttackAnimation(float attackDuration)
        {
            _attackDuration = attackDuration;
        }
        public void Init(AttackComponentInitData data)
        {
            _attackInterval = data.Unit.Model.AttackModel.AttackInterval;
            var animator = data.AttackRoot.gameObject.RequireComponentInChildren<Animator>();
            _blendShapeAnimation = data.AttackRoot.gameObject.RequireComponentInChildren<BlendShapeAnimation>();
            _animatorTween = AnimatorTween.Create(animator.gameObject);
            PlayInflateBellyAnimation();
        }
        
        public void Play()
        {
            _startedAnimation = true;
            _animatorTween.WaitForEventFromBool(_attackHash, true, _startingAttackStateHash, OnFireStarted)
                .AddTo(Disposable);
        }

        public void Interrupt() => FinishAttackAnimation();

        private void FinishAttackAnimation()
        {
            DisposeAnimation();
            _animatorTween.WaitForEventFromBool(_attackHash, false, _finishingAttackStateHash, OnAnimationCompleted)
                .AddTo(Disposable);
        }

        private void OnFireStarted()
        {
            DisposeAnimation();
            SendFireEvent();
            PlayDeflateBellyAnimation();
            Observable.Timer(TimeSpan.FromSeconds(_attackDuration)).Subscribe(_=> FinishAttackAnimation())
                .AddTo(Disposable);
            
        }
        
        private void SendFireEvent() => OnFire?.Invoke();

        private void OnAnimationCompleted()
        {
            _startedAnimation = false;
            OnFireAnimationFinished?.Invoke();
            DisposeAnimation();
            PlayInflateBellyAnimation();
        }

        private void PlayInflateBellyAnimation() => _blendShapeAnimation.Play(_blendShapeAnimation.MinValue, _attackInterval);

        private void PlayDeflateBellyAnimation() => _blendShapeAnimation.Play(_blendShapeAnimation.MaxValue, _attackDuration);


        private void DisposeAnimation()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public void Dispose()
        {
            DisposeAnimation();
        }
        
    }
}