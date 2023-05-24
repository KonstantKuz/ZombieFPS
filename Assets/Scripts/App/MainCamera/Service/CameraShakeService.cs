using App.MainCamera.Config;
using App.Player.Messages;
using DG.Tweening;
using Feofun.Extension;
using Feofun.World;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.MainCamera.Service
{
    public class CameraShakeService : IWorldScope
    {
        private CompositeDisposable _disposable;
        private Transform _cameraTransform;
        private Animator _cameraAnimator;
        
        [Inject] private IMessenger _messenger;
        [Inject] private CameraShakeConfig _cameraShakeConfig;

        public Transform CameraTransform => _cameraTransform ??= Camera.main.transform;
        public Animator CameraAnimator => _cameraAnimator ??= CameraTransform.GetComponent<Animator>();

        public void OnWorldSetup()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _messenger.SubscribeWithDisposable<PlayerProjectileHitMessage>(msg => 
                PlayShake(msg.ShakeEventType.ToString())).AddTo(_disposable);
            // _messenger.SubscribeWithDisposable<PlayerFireMessage>(msg => 
            // PlayShake($"{ShakeEventType.PlayerShoot}{msg.WeaponName}")).AddTo(_disposable);
            _messenger.SubscribeWithDisposable<PlayerDamagedMessage>(msg => 
                PlayShake($"{ShakeEventType.PlayerDamaged}{msg.AttackName}")).AddTo(_disposable);
        }

        public void PlayShake(string eventName)
        {
            var shakeLevel = _cameraShakeConfig.FindShakeConfig(eventName);
            if(shakeLevel == null) return;
            if (shakeLevel.AnimationClip != null)
            {
                PlayAnimationShake(shakeLevel.AnimationClip);
            }
            else
            {
                PlayTweenShake(shakeLevel);
            }
        }

        private void PlayAnimationShake(AnimationClip animationClip)
        {
            CameraAnimator.Play(animationClip.name);
        }

        private void PlayTweenShake(CameraShakeLevel shakeLevel)
        {
            var shake = CameraTransform.parent.DOShakePosition(shakeLevel.Duration, shakeLevel.Force, (int) shakeLevel.Frequency);
            shake.ToDisposable().AddTo(_disposable);
        }

        public void OnWorldCleanUp()
        {
            Dispose();
        }

        private void Dispose()
        {
            _cameraTransform = null;
            _cameraAnimator = null;
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
