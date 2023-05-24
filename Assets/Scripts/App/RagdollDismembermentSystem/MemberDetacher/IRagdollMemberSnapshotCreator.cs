using System.Collections.Generic;
using App.RagdollDismembermentSystem.Data;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public interface IRagdollMemberSnapshotCreator : IDismembererComponent
    {
        void CreateSnapshots(ICollection<DismembermentFragment> fragments);
    }
}