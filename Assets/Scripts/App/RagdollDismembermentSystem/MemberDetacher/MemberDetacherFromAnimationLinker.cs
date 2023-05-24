using App.RagdollDismembermentSystem.Component;
using App.RagdollDismembermentSystem.Data;
using Feofun.Extension;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public class MemberDetacherFromAnimationLinker : MonoBehaviour, IRagdollMemberDetacher, IRagdollInitStateRecoverer
    {
        private RagdollAnimationLinker _ragdollAnimationLinker;

        private void Awake()
        {
            _ragdollAnimationLinker = gameObject.RequireComponent<RagdollAnimationLinker>();
        }

        public void RecoverInitState() => _ragdollAnimationLinker.Init();

        public void DetachFromBody(DismembermentFragment fragment)
        {
            _ragdollAnimationLinker.RemoveLink(fragment.CrackedBoneTransform);
        }
    }
}