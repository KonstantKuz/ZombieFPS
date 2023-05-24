using System;
using App.Unit.Component.Health;
using App.Unit.Component.Health.Extension;
using UniRx;
using UnityEngine;

namespace App.UI.Components.FrameEffects.Effects.HealthFrameEffect
{
    public class HealthFrameModel
    {
        public readonly IObservable<float> FadePercent;
        public readonly IObservable<bool> IsDark;

        public HealthFrameModel(IHealthOwner owner)
        {
            FadePercent = owner.GetHealthPercent().Select(it =>  Mathf.Clamp01(1 - it));
            IsDark = FadePercent.Select(it => it >= 1);
        }
    }
}