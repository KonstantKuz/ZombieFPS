using System.Collections.Generic;
using System.Linq;
using Feofun.World.Factory.ObjectFactory;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Feofun.Vfx
{
    public class PooledParticleSystemVfx : MonoBehaviour
    {
        private ParticleSystem[] _particlesWithoutSubEmitters;
        private readonly CompositeDisposable _disposable = new();
        
        [Inject(Id = ObjectFactoryType.Pool)]
        private IObjectFactory _objectFactory;
    
        private void Awake()
        {
            var particles = GetComponentsInChildren<ParticleSystem>();
            _particlesWithoutSubEmitters = GetParticleSystemsWithoutSubEmitters(particles).ToArray();
            _particlesWithoutSubEmitters.ForEach(SetupStopAction);

            particles
                .Where(it => it.main.stopAction == ParticleSystemStopAction.Callback)
                .Select(it => it.OnParticleCompleteAsObservable())
                .Zip()
                .Subscribe(it => Destroy()).AddTo(_disposable);
        }

        private static IEnumerable<ParticleSystem> GetParticleSystemsWithoutSubEmitters(ICollection<ParticleSystem> particles)
        {
            var subEmitters = particles
                .Select(it => it.subEmitters)
                .SelectMany(it => Enumerable.Range(0, it.subEmittersCount)
                    .Select(it.GetSubEmitterSystem));
            return particles.Except(subEmitters);
        }

        private void OnEnable()
        {
            _particlesWithoutSubEmitters.ForEach(it => it.Play());
        }

        private static void SetupStopAction(ParticleSystem ps)
        {
            var mainModule = ps.main;
            if (mainModule.stopAction == ParticleSystemStopAction.Destroy)
            {
                mainModule.stopAction = ParticleSystemStopAction.Callback;
            }
        }

        private void Destroy()
        {
            _objectFactory.Destroy(gameObject);
        }
    }
}