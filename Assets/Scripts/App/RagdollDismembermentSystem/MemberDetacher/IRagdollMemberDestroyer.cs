using App.RagdollDismembermentSystem.Data;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public interface IRagdollMemberDestroyer : IDismembererComponent
    {
        void OnDestroyFragments(DismembermentFragment fragment);
    }
}