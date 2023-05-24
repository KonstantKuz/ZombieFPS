using System;
using App.Animation;
using App.Unit.Component.Attack;
using App.Weapon.Component;
using Feofun.Components;
using Feofun.Extension;
using Logger.Extension;
using UnityEngine;

namespace App.Player.Component.Attack.Reloader
{
    public class OneShotWeaponReloader : WeaponReloaderBase, IInitializable<AttackComponentInitData>
    {
        private const string RELOADED_ANIMATION_EVENT = "Reloaded";
        private static readonly int _startReloadingParamHash = Animator.StringToHash("Reload");
        private static readonly int _finishReloadingParamHash = Animator.StringToHash("FinishReload");
        
        private readonly string _startAnimationName;
        private readonly string _mainAnimationName;

        private Animator _animator;
        private AnimationEventHandler _eventHandler;
        private bool _unloadClipBeforeReload;
        
        private float OneAmmoReloadTime => _weaponState.Model.ReloadTime / Clip.Size ;
        

        public OneShotWeaponReloader(RuntimeInventoryWeaponState weaponState,
            string startAnimationName,
            string mainAnimationName, bool unloadClipBeforeReload) : base(weaponState)
        {
            _startAnimationName = startAnimationName;
            _mainAnimationName = mainAnimationName;
            _unloadClipBeforeReload = unloadClipBeforeReload;
        }

        public void Init(AttackComponentInitData data)
        {
            _animator = data.AttackRoot.gameObject.RequireComponentInChildren<Animator>();
            _eventHandler = _animator.gameObject.RequireComponent<AnimationEventHandler>();
            _eventHandler.OnEvent += OnAnimationEvent;
            FullClipReloadingAnimation.TryUpdateAnimationSpeed(_animator, OneAmmoReloadTime, _mainAnimationName, this.Logger());
        }

        private void OnAnimationEvent(string eventName)
        {
            if (!eventName.Equals(RELOADED_ANIMATION_EVENT)) return;
            Clip.Load(1);
        }

        public override void StartReload()
        {
            DisposeTimer();
            if (_unloadClipBeforeReload) {
                Clip.UnloadAll();  
            }
            var needAmmoCount = Clip.Size - Clip.AmmoCount.Value;
            var reloadTime = (needAmmoCount * OneAmmoReloadTime) + GetClipLength(_animator, _startAnimationName);
            
            StartTimer(reloadTime);
            _animator.SetTrigger(_startReloadingParamHash);
        }
        private static float GetClipLength(Animator animator, string animationName)
        {
            var clip = FullClipReloadingAnimation.FindAnimationClip(animator, animationName);
            if (clip == null) {
                throw new Exception("The OneShotWeaponReloader component does " +
                                    $"not find the animation for start reloading, name:= {animationName}");
            }
            return clip.length;
        }

        public override void StopReload()
        {
            _animator.SetTrigger(_finishReloadingParamHash);
            base.StopReload();
        }

        protected override void OnReloaded()
        {
            DisposeTimer();
            _animator.SetTrigger(_finishReloadingParamHash);
        }

        public override void Dispose()
        {
            _eventHandler.OnEvent -= OnAnimationEvent;
            base.Dispose();
        }

    }
}