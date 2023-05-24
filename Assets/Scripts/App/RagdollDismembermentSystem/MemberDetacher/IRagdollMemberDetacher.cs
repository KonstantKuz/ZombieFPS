using App.RagdollDismembermentSystem.Data;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public interface IRagdollMemberDetacher : IDismembererComponent
    {
        void DetachFromBody(DismembermentFragment fragment);
    }
}