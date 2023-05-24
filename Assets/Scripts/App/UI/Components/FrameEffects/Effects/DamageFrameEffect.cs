using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Components.FrameEffects.Effects
{
    public class DamageFrameEffect : FrameEffect
    {
        [SerializeField] private float _appearTime;    
        [SerializeField] private float _holdTime;
        [SerializeField] private float _disappearTime;
        [SerializeField] private Image _damageFrame;

        private Sequence _frameAnimation;
        
        public override void StartEffect(Action onComplete)
        {
            base.StartEffect(onComplete);
            _frameAnimation = DOTween.Sequence();
            _frameAnimation.Append(_damageFrame.DOFade(1, _appearTime));  
            _frameAnimation.AppendInterval(_holdTime);
            _frameAnimation.Append(_damageFrame.DOFade(0, _disappearTime));
            _frameAnimation.OnComplete(NotifyStop);
        }

        public override void StopEffect()
        {
            base.StopEffect();
            _frameAnimation?.Kill(true);
        }
    }
}
