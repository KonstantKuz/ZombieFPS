using System;
using App.PhysicsInternal;
using App.Ragdoll;
using App.Ragdoll.PhysicsActivator;
using RootMotion.Dynamics;

namespace App.Extension
{
    public static class PuppetExt
    {
        public static void Activate(this PuppetMaster puppetMaster)
        {
            puppetMaster.mode = PuppetMaster.Mode.Active;
            puppetMaster.enabled = true;
        }

        public static void Deactivate(this PuppetMaster puppetMaster, PuppetDeactivateMode deactivateMode)
        {
            switch (deactivateMode)
            {
                case PuppetDeactivateMode.SetKinematic:
                    puppetMaster.DeactivateBySetKinematic();
                    break;
                case PuppetDeactivateMode.DeactivateCompletely:
                    puppetMaster.DeactivateCompletely();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deactivateMode), deactivateMode, null);
            }
        }

        public static void DeactivateCompletely(this PuppetMaster puppetMaster)
        {
            puppetMaster.DisableImmediately();
            puppetMaster.enabled = false;
        }

        public static void DeactivateBySetKinematic(this PuppetMaster puppetMaster)
        {
            puppetMaster.mode = PuppetMaster.Mode.Kinematic;
            puppetMaster.enabled = true;
        }
    }
}