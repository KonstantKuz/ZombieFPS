using App.InteractableItems.Model;
using Zenject;

namespace App.InteractableItems.Service
{
    public class InteractableRewardService
    {
        [Inject] private InteractableRewardRepository _repository;

        private InteractableRewardState State => _repository.Get() ?? new InteractableRewardState();

        public bool IsRewardGiven(string rewardId)
        {
            if (!State.Rewards.ContainsKey(rewardId))
            {
                var state = State;
                state.Rewards.Add(rewardId, false);
                _repository.Set(state);
            }
            return State.Rewards[rewardId];
        }

        public void SetRewardGiven(string rewardId)
        {
            var state = State;
            State.Rewards[rewardId] = true;
            _repository.Set(state);
        }
    }
}
