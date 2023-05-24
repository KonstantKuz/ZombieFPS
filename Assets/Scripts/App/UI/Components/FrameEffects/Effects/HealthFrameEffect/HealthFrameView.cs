using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Components.FrameEffects.Effects.HealthFrameEffect
{
    public class HealthFrameView : MonoBehaviour
    {
        [SerializeField] private Image _frame;
        [SerializeField] private float _animationTime = 0.5f;

        private Color _initialColor;
        
        private Tweener _fadeTween; 
        private Tweener _colorTween;
        
        private CompositeDisposable _disposable;

        private void Awake()
        {
            _initialColor = _frame.color;
        }
        
        public void Init(HealthFrameModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            model.FadePercent.Subscribe(OnUpdateFade).AddTo(_disposable);
            model.IsDark.Subscribe(OnUpdateColor).AddTo(_disposable);
        }
        private void OnUpdateColor(bool isDark)
        {
            if(!isDark) return;
            _colorTween?.Kill(true);
            _colorTween = _frame.DOColor(Color.black, _animationTime);
        }
        private void OnUpdateFade(float fadePercent)
        {
            _fadeTween?.Kill(true);
            _fadeTween = _frame.DOFade(fadePercent, _animationTime);
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _fadeTween?.Kill(true);
            _colorTween?.Kill(true);
            _disposable?.Dispose();
            ResetState();
        }

        private void ResetState()
        {
            _frame.color = _initialColor;
        }
    }
}