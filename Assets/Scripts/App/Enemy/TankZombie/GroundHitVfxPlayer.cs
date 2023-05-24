using App.Animation;
using App.Unit.Component.Animation;
using Feofun.World.Factory.ObjectFactory.Factories;
using UnityEngine;
using Zenject;

namespace App.Enemy.TankZombie
{
    public class GroundHitVfxPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _vfx;
        [SerializeField] private Transform _vfxPosition;
        
        [Inject] private ObjectInstancingFactory _objectFactory;

        private AnimationEventHandler _eventHandler;

        private void Awake()
        {
            _eventHandler = GetComponentInChildren<AnimationEventHandler>();
            _eventHandler.OnEvent += OnEvent;
        }

        private void OnDestroy()
        {
            _eventHandler.OnEvent -= OnEvent;
        }

        private void OnEvent(string eventName)
        {
            if (eventName != AnimationEvents.GROUND_HIT_VFX) return;
            var vfx = _objectFactory.Create<ParticleSystem>(_vfx);
            vfx.transform.SetPositionAndRotation(_vfxPosition.transform.position, _vfxPosition.transform.rotation);
            vfx.Play();
        }
    }
}