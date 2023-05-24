using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Feofun.Vfx
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    public class ObservableParticleCompleteTrigger : ObservableTriggerBase
    {
        private Subject<ParticleSystem> _onParticleStopped;
        private ParticleSystem _particleSystem;

        private ParticleSystem ParticleSystem => _particleSystem ??= GetComponent<ParticleSystem>();
        
        public IObservable<ParticleSystem> OnParticleAsAsObservable()
        {
            return _onParticleStopped ??= new Subject<ParticleSystem>();
        }

        private void OnParticleSystemStopped()
        {
            _onParticleStopped?.OnNext(ParticleSystem);
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            Complete();
        }

        private void Complete()
        {
            _onParticleStopped?.OnCompleted();
            _onParticleStopped = null;
        }
    }
}