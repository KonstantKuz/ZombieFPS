using App.RagdollDismembermentSystem.Component;
using Feofun.Extension;

namespace App.Ragdoll
{
    public class RagdollWithAnimationLinker : RagdollBase
    {
        private RagdollAnimationLinker _ragdollAnimationLinker;
        
        private void Awake()
        {
            _ragdollAnimationLinker = gameObject.RequireComponentInChildren<RagdollAnimationLinker>();
            Disable();
        }

        public override void Enable(bool switchAnimator = true)
        {
            base.Enable(switchAnimator);
            _ragdollAnimationLinker.enabled = false;
        }
        
        public override void Disable(bool switchAnimator = true)
        {
            base.Disable(switchAnimator);
            _ragdollAnimationLinker.enabled = true;
        }
    }
}