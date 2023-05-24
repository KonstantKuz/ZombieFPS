using App.Extension;
using RootMotion.Dynamics;
using UnityEngine;

namespace App.Ragdoll.PhysicsActivator
{
    public enum PuppetDeactivateMode
    {
        SetKinematic,
        DeactivateCompletely,
    }
    public class PuppetRagdollPhysicsActivator : RagdollPhysicsActivator
    {
        [SerializeField] private PuppetDeactivateMode _deactivateMode;
        
        private PuppetMaster _puppetMaster;
        private PuppetMaster PuppetMaster => _puppetMaster ??= GetComponentInChildren<PuppetMaster>(true);
        
        protected override void SetPhysicsEnabled(bool value)
        {
            if (value) {
                PuppetMaster.Activate();
                PuppetMaster.OnPostSimulate();
            }else {
                PuppetMaster.Deactivate(_deactivateMode);
            }
            base.SetPhysicsEnabled(value);
        }
        
    }
}