using System;
using Feofun.UI.Components.Animated;
using UnityEngine;

namespace App.UI.Screen.World.Player.Health
{
    public class ScaledProgressBarView: ProgressBarView
    {
        private float _initialHeight;
        
        private void Awake()
        {
            _initialHeight = (transform as RectTransform).rect.height;
        }

        public override float Value
        {
            get => (transform as RectTransform).rect.height / _initialHeight;
            protected set
            {
                var rectTransform = (transform as RectTransform);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -(1f - value) * _initialHeight);
            }
        }
    }
}