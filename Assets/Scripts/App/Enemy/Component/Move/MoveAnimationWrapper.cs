using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace App.Enemy.Component.Move
{
    public class MoveAnimationWrapper
    {
        private const float SMOOTH_TRANSITION_TIME = 0.2f;
        
        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _cycleOffsetHash = Animator.StringToHash("CycleOffset");

        private readonly Animator _animator;
        private readonly CompositeDisposable _disposable;
        private Tweener _activeAnimation;
        public MoveAnimationWrapper(Animator animator)
        {
            _animator = animator;
            _disposable = new CompositeDisposable();
            _animator.gameObject.OnDestroyAsObservable().Subscribe(it => Dispose()).AddTo(_disposable);
        }

        public void PlayIdleSmooth() => AnimateMotionValues(0);

        public void PlayMoveForwardSmooth() => AnimateMotionValues(1);

        public void SetRandomTimeOfCurrentState() => _animator.SetFloat(_cycleOffsetHash, Random.Range(0f, 1f));

        private void AnimateMotionValues(float vertical) => SmoothTransition(_verticalMotionHash, vertical, SMOOTH_TRANSITION_TIME);

        private void SmoothTransition(int animationHash, float toValue, float time)
        {
            DisposeAnimation();
            _activeAnimation = DOTween.To(() => _animator.GetFloat(animationHash), value => _animator.SetFloat(animationHash, value), toValue, time);
        }
        
        private void DisposeAnimation() => _activeAnimation?.Kill();

        private void Dispose()
        {
            DisposeAnimation();
            _disposable?.Dispose();
        }
    }
}