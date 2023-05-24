using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Animation
{
    public class AnimatedTransparencyMaterialWrapper
    {
        [CanBeNull] private readonly string _colorName;
        private readonly bool _isTransparentOnInit;
        private readonly Color _initialColor;
        private readonly Color _clearColor;
        private readonly Material _material;

        public AnimatedTransparencyMaterialWrapper(Material it, string colorPropertyName)
        {
            var initialColor = it.HasColor(colorPropertyName) ? MaterialExt.GetColor(it, colorPropertyName) : it.color;
            _colorName = it.HasColor(colorPropertyName) ? colorPropertyName : null;
            _isTransparentOnInit = it.IsTransparentOrFade();
            _initialColor = initialColor;
            _clearColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
            _material = it;
        }
        
        public void SetTransparent()
        {
            _material.SetColor(_clearColor, _colorName);
            if (_isTransparentOnInit) return;
            _material.ToFade();
        }
            
        public void TweenColors(float progress)
        {
            var color = Color.Lerp(_clearColor, _initialColor, progress);
            MaterialExt.SetColor(_material, color, _colorName);
            if(progress < 1f) return;
            if(_isTransparentOnInit) return;
            Restore();
        }

        public void Restore()
        {
            MaterialExt.SetColor(_material, _initialColor, _colorName);
            if(_isTransparentOnInit) return;
            _material.ToOpaque();
        }
    }
}