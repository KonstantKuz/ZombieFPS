using System.Collections.Generic;
using App.RagdollDismembermentSystem.Data;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public interface IRagdollMemberRecoverer : IDismembererComponent
    {
        void RecoverFragments(ICollection<DismembermentFragment> fragments);
    }
}