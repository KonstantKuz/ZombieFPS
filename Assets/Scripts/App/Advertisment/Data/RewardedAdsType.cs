namespace App.Advertisment.Data
{
    public enum RewardedAdsType
    {
        Booster,
        Item,
    }

    public static class RewardedAdsTypeExt
    {
        public static string CreateRewardedPlacementId(this RewardedAdsType rewardedAdsType, string rewardName)
        {
            return rewardedAdsType + "_" + rewardName;
        }
    }
}