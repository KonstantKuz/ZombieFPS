using Feofun.Repository;

namespace App.InteractableItems.Model
{
    public class InteractableRewardRepository : LocalPrefsSingleRepository<InteractableRewardState>
    {
        public InteractableRewardRepository() : base("reward") { }
    }
}
