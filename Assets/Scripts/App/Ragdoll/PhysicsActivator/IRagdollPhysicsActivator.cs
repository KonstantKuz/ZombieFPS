namespace App.Ragdoll.PhysicsActivator
{
    public interface IRagdollPhysicsActivator
    {
        ActivationMode ActivationMode { get; set; }
        void Activate();
        void Deactivate();
    }
}