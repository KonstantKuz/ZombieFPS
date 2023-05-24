namespace App.Reward
{
    public class RewardItem
    {
        public readonly string Id;
        public readonly RewardType Type;
        
        public RewardItem(string id, RewardType type)
        {
            Id = id;
            Type = type;
        }
    }
}