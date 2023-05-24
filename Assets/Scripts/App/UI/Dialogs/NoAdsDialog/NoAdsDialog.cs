using DG.Tweening;
using Feofun.UI.Dialog;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.NoAdsDialog
{
    public class NoAdsDialog : BaseDialog
    {
        private const int OFFSET = 50;
        private const float ANIMATION_TIME = 0.5f;
        
        [SerializeField] private CanvasGroup _canvasGroup;

        private RectTransform _rectTransform;
        private Sequence _animation;
        
        public void OnEnable()
        {
            _rectTransform = _canvasGroup.transform as RectTransform;
            ResetState();
            PlayAnimation();
        }

        private void ResetState()
        {
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = -Vector3.up * OFFSET;
        }

        private void PlayAnimation()
        {
            _animation?.Kill();
            _animation = DOTween.Sequence();
            _animation.Append(_canvasGroup.DOFade(1f, ANIMATION_TIME));
            _animation.Join(_rectTransform.DOAnchorPosY(0, ANIMATION_TIME));
            _animation.AppendInterval(ANIMATION_TIME);
            _animation.onComplete = HideDialog;
        }
        
    }
}
