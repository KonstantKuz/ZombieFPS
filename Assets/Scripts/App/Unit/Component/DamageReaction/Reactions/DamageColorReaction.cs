using System;
using App.Unit.Component.Health;
using DG.Tweening;
using UnityEngine;

namespace App.Unit.Component.DamageReaction.Reactions
{
    public class DamageColorReaction : MonoBehaviour, IDamageReaction, IDisposable
    {
        [SerializeField]
        private float _colorBlinkDuration;
        [SerializeField]
        private Color _blinkColor;
        [SerializeField]
        private Renderer[] _renderers;
        [SerializeField]
        private string _colorPropertyName = "_Color";
        
        private Sequence _colorBlink;

        public void OnDamageReaction(DamageInfo damage)
        {
            _colorBlink?.Kill(true);

            _colorBlink = DOTween.Sequence();
            foreach (var renderer1 in _renderers)
            {
                foreach (var material in renderer1.materials)
                {
                    _colorBlink.Join(DoBlink(material));
                }
            }

            _colorBlink.Play();
        }

        private Tween DoBlink(Material material)
        {
            var initialColor = material.GetColor(_colorPropertyName);
            var sequence = DOTween.Sequence();
            sequence.Append(material.DOColor(_blinkColor, _colorPropertyName, _colorBlinkDuration)
                .SetEase(Ease.OutCubic));
            sequence.Append(material.DOColor(initialColor, _colorPropertyName, _colorBlinkDuration)
                .SetEase(Ease.InCubic));
            return sequence;
        }

        public void Dispose()
        { 
            _colorBlink?.Kill(true);
        }
    }
}