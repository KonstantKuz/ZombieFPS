using System;
using System.Collections.Generic;
using System.Linq;
using App.PhysicsInternal;
using App.RagdollDismembermentSystem.Data;
using App.RagdollDismembermentSystem.MemberDetacher;
using Feofun.Extension;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Ragdoll.PhysicsActivator
{
    public enum ActivationMode
    {
        Temporary,
        Continuous,
    }

    public class RagdollPhysicsActivator : MonoBehaviour, IRagdollPhysicsActivator, IRagdollMemberRecoverer,
        IRagdollMemberDetacher
    {
        private const float TEMPORARY_ACTIVATING_AVALIABLE_TIMEOUT = 0.1f;
        
        [SerializeField] 
        private float _activeTimeOnHit = 1f;
        
        [Inject]
        private TemporaryActiveRagdollService _temporaryActiveRagdollService;
        
        private IDisposable _timerDisposable;
        private float _lastActivatingTime;
        private IDictionary<Rigidbody, RigidbodyPhysicsActivator> _rigidbodiesActivators;
        private IDictionary<Rigidbody, RigidbodyPhysicsActivator> _backupRigidbodiesActivators;
     
        
        public ActivationMode ActivationMode { get; set; } = ActivationMode.Temporary;
        
        private IDictionary<Rigidbody, RigidbodyPhysicsActivator> RigidbodiesActivators
        {
            get
            {
                if (_rigidbodiesActivators == null) {
                    InitRigidbodiesActivators();
                }
                return _rigidbodiesActivators;
            }
            set => _rigidbodiesActivators = value;
        }

        private void InitRigidbodiesActivators()
        {
            _rigidbodiesActivators = GetComponentsInChildren<Rigidbody>(true)
                .ToDictionary(it => it, it =>
                    it.gameObject.GetOrAddComponent<RigidbodyPhysicsActivator>().Init());
            _backupRigidbodiesActivators = _rigidbodiesActivators
                .ToDictionary(it => it.Key, it => it.Value);
        }

        public void RecoverFragments(ICollection<DismembermentFragment> fragments)
        {
            RigidbodiesActivators = _backupRigidbodiesActivators.ToDictionary(it => it.Key, 
                it => it.Value);;
            foreach (var physicsActivator in RigidbodiesActivators.Values)
            {
                physicsActivator.RecoverLinkWithParent();
                physicsActivator.SetKinematic(true);
            }
        }
        
        public void DetachFromBody(DismembermentFragment fragment)
        {
            foreach (var boneRigidbody in fragment.BonesRigidbodies)
            {
                var physicsActivator = RigidbodiesActivators[boneRigidbody];
                physicsActivator.SetKinematic(false);
                physicsActivator.BreakLinkWithParent();
                RigidbodiesActivators.Remove(boneRigidbody);
            }
        }
        
        public void Activate()
        {
            switch (ActivationMode)
            {
                case ActivationMode.Temporary:
                    ActivateTemporary(_activeTimeOnHit);
                    return;
                case ActivationMode.Continuous:
                    ActivateContinuous();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ActivationMode), ActivationMode, null);
            }
        }
        
        public void Deactivate()
        {
            DisposeTimer();
            _temporaryActiveRagdollService.Remove(this);
            SetPhysicsEnabled(false);
        }

        private void ActivateContinuous()
        {
            _temporaryActiveRagdollService.Remove(this);
            DisposeTimer();
            SetPhysicsEnabled(true);
        }
        
        private void ActivateTemporary(float activeSeconds)
        {  
            if (Math.Abs(Time.time - _lastActivatingTime) < TEMPORARY_ACTIVATING_AVALIABLE_TIMEOUT) {
                return;
            }
            SetPhysicsEnabled(true);
            _temporaryActiveRagdollService.Add(this);
            DisposeTimer();
            _lastActivatingTime = Time.time;
            _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(activeSeconds))
                .Subscribe(it => Deactivate());
        }

        private void DisposeTimer()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
        }

        protected virtual void SetPhysicsEnabled(bool value)
        {
            foreach (var physicsActivator in RigidbodiesActivators.Values) {
                physicsActivator.SetKinematic(!value);
            }
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            Deactivate();
            ActivationMode = ActivationMode.Temporary;
        }


    }
}