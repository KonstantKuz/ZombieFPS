using System;
using Feofun.Extension;
using UniRx;
using UnityEngine;

namespace Feofun.Vfx
{
    public static class ParticleSystemExt
    {
        public static IObservable<ParticleSystem> OnParticleCompleteAsObservable(this Component component)
        {
            var trigger = component.gameObject.GetOrAddComponent<ObservableParticleCompleteTrigger>();
            return trigger.OnParticleAsAsObservable();
        }        
    }
}