using System;
using System.Linq;
using App.Unit.Component.Attack;
using App.Weapon.Component;
using Feofun.Components;
using Feofun.Extension;
using Feofun.Util.Timer;
using JetBrains.Annotations;
using Logger.Extension;
using UniRx;
using UnityEngine;

namespace App.Player.Component.Attack.Reloader
{
    public class FullClipReloadingAnimation : AttackComponent, IInitializable<AttackComponentInitData>, IDisposable
    {
        private static readonly int _reloadingParamHash = Animator.StringToHash("Reload");
        private static readonly int _reloadSpeedMultiplierHash = Animator.StringToHash("ReloadSpeedMultiplier");

        private readonly RuntimeInventoryWeaponState _weaponState;
        private readonly IWeaponReloader _weaponReloader;
        private readonly string _animationName;

        private Animator _animator;
        private CompositeDisposable _disposable;
        
        public FullClipReloadingAnimation(RuntimeInventoryWeaponState weaponState, IWeaponReloader weaponReloader,
            string animationName)
        {
            _weaponState = weaponState;
            _animationName = animationName;
            _weaponReloader = weaponReloader;
        }

        public void Init(AttackComponentInitData data)
        {
            _animator = data.AttackRoot.gameObject.RequireComponentInChildren<Animator>();
            Dispose();
            _disposable = new CompositeDisposable();
            TryUpdateAnimationSpeed(_animator, _weaponState.Model.ReloadTime, _animationName, this.Logger());
            _weaponReloader.ReloadingTimer.Subscribe(OnUpdateReloading).AddTo(_disposable);
        }

        private void OnUpdateReloading([CanBeNull] ITimer timer)
        {
            if (timer == null) return;
            _animator.SetTrigger(_reloadingParamHash);
        }

        public static void TryUpdateAnimationSpeed(Animator animator, float reloadTime, string animationName, Logger.ILogger logger)
        {
            var clip = FindAnimationClip(animator, animationName);
            if (clip == null) 
            {
                logger.Warn($"The ReloadingAnimation component does " +
                            $"not find the animation for reloading, name:= {animationName}");
                return;
            }
            animator.SetFloat(_reloadSpeedMultiplierHash,  clip.length / reloadTime);
        }
        
        [CanBeNull]
        public static AnimationClip FindAnimationClip(Animator animator, string animationName)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            return clips.FirstOrDefault(it => it.name == animationName);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}