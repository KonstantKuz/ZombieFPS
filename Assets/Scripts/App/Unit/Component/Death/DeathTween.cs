using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.Unit.Component.Death
{
    public enum DeathTweenType
    {
        FallDown,
        ScaleDown,
        Disappear,
    }
    public class DeathTween : MonoBehaviour
    {
        [SerializeField]
        private DeathTweenType _tweenType;
        [SerializeField]
        private float _disappearTime;
        [SerializeField]
        private float _offsetYDisappear;
        
        private Tween _deathTween;
        private Renderer[] _renderers;
        
        private void Awake()
        {
            _renderers = gameObject.GetComponentsInChildren<Renderer>();
        }
        public IEnumerator Play()
        {
            EndTweenIfStarted();
            
            _deathTween = CreateDeathTween();
            yield return _deathTween.WaitForCompletion();
        }
        private Tween CreateDeathTween()
        {
            switch (_tweenType)
            {
                case DeathTweenType.FallDown:
                    return gameObject.transform.DOMoveY(transform.position.y - _offsetYDisappear, _disappearTime);
                case DeathTweenType.ScaleDown:
                    return gameObject.transform.DOScale(0, _disappearTime);
                case DeathTweenType.Disappear:
                    return CreateDisappearTween();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private Tween CreateDisappearTween()
        {
            var materials = _renderers.SelectMany(it=>it.materials);
            materials.ForEach(it=>it.ToTransparent());
            var tweenSequence = DOTween.Sequence();
            materials.ForEach(it => tweenSequence.Insert(0f, it.DOFade(0, _disappearTime)));
            return tweenSequence;
        }
        private void EndTweenIfStarted()
        {
            if (_deathTween == null) return;
            _deathTween.Kill(true);
            _deathTween = null;
        }
        private void OnDisable() => EndTweenIfStarted();
    }
}