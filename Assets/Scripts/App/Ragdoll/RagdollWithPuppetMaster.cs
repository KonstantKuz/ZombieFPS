using System;
using Feofun.Extension;
using RootMotion.Dynamics;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;

namespace App.Ragdoll
{
    public class RagdollWithPuppetMaster : RagdollBase
    {
        [SerializeField] private float _deadBodyDisableTimeout = 2f;
        [SerializeField] private bool _isDisablingDeadBody;
        
        private PuppetMaster _puppetMaster;
        private IDisposable _disableTimer;
        
        private void Awake()
        {
            _puppetMaster = gameObject.RequireComponentInChildren<PuppetMaster>();
            _puppetMaster.OnPostInitiate += OnPuppetInit;
            _puppetMaster.OnDeath += DisableWithTimeout;
        }

        private void OnPuppetInit()
        {
            PreventPuppetFromStretching();
            Disable();
        }
        
        private void DisableWithTimeout()
        {
            DisposeTimer();
            if(!_isDisablingDeadBody) return;
            _disableTimer = Observable.Timer(TimeSpan.FromSeconds(_deadBodyDisableTimeout))
                .Subscribe(it => {
                    DisposeTimer();
                    RagdollPhysicsActivator.Deactivate();
                });
        }

        private void PreventPuppetFromStretching()
        {
            GetComponentsInChildren<ConfigurableJoint>()
                .ForEach(it => { it.projectionMode = JointProjectionMode.PositionAndRotation; });
        }
        
        public override void Enable(bool switchAnimator = true)
        {
            DisposeTimer();
            _puppetMaster.state = PuppetMaster.State.Dead;
            base.Enable(switchAnimator);
        }
        
        public override void Disable(bool switchAnimator = true)
        {
            DisposeTimer();
            _puppetMaster.state = PuppetMaster.State.Alive;
            base.Disable(switchAnimator);
        }
        
        private void DisposeTimer()
        {
            _disableTimer?.Dispose();
            _disableTimer = null;
        }
        
        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            DisposeTimer();
            _puppetMaster.OnPostInitiate -= OnPuppetInit;
            _puppetMaster.OnDeath -= DisableWithTimeout;
        }


    }
}