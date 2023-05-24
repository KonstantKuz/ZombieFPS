using App.Animation;
using App.Unit.Component.Animation;
using UnityEngine;

namespace App.Enemy.TankZombie
{
    public class TankZombieProjectileVisibilityHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        private AnimationEventHandler _eventHandler;
        private void Awake()
        {
            _projectile.SetActive(false);
            _eventHandler = GetComponentInChildren<AnimationEventHandler>();
            _eventHandler.OnEvent += OnAnimationEvent;
        }

        private void OnDestroy()
        {
            _eventHandler.OnEvent -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case AnimationEvents.SHOW_PROJECTILE:
                    _projectile.SetActive(true);
                    break;
                case AnimationEvents.FIRE:
                    _projectile.SetActive(false);
                    break;
            }
        }
    }
}