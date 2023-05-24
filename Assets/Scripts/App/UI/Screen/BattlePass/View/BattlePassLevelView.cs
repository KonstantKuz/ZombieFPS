using App.BattlePass.Model;
using App.UI.Screen.BattlePass.Model;
using App.UI.Util;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Screen.BattlePass.View
{
    public class BattlePassLevelView : MonoBehaviour
    {
        [SerializeField] private AnimationParams _animationParams;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _fadeGroup;
        [SerializeField] private Image _progressBar;

        private BattlePassLevelModel _model;
        private Sequence _animation;
        
        public void Init(BattlePassLevelModel model)
        {
            _model = model;
            _icon.sprite = IconLoader.LoadItemIcon(model.RewardItem.Id);
            _text.SetText(model.Level.ToString());
            _fadeGroup.alpha = GetFade(model.State);
            _progressBar.fillAmount = GetProgress(model.State);
            model.ReceivingAnimationCallback = PlayReceiveAnimation;
        }

        private float GetFade(BattlePassRewardState state)
        {
            return state == BattlePassRewardState.Taken ? 1f : _animationParams.NonReceivedFade;
        }

        private float GetProgress(BattlePassRewardState state)
        {
            return state == BattlePassRewardState.Taken ? 1f : 0f;
        }

        private void PlayReceiveAnimation()
        {
            ResetState();
            
            _animation = DOTween.Sequence();
            _animation.AppendInterval(_animationParams.AnimationDelay);
            var doProgress = _progressBar.DOFillAmount(1f, _animationParams.AnimationDuration);
            _animation.Append(doProgress);
            _animation.onComplete = OnComplete;
        }

        private void OnComplete()
        {
            _fadeGroup.alpha = 1f;
            _model.OnReceivingAnimationComplete?.Invoke();
        }
        
        private void ResetState()
        {
            _animation?.Kill();
            _progressBar.fillAmount = 0f;
            _fadeGroup.alpha = _animationParams.NonReceivedFade;
        }
        
        private void OnDisable()
        {
            ResetState();
        }

        [System.Serializable]
        public class AnimationParams
        {
            public float NonReceivedFade = 0.3f;
            public float AnimationDelay = 0.5f;
            public float AnimationDuration = 0.5f;
        }
    }
}