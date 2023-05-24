using Feofun.Advertisment.Data;
using RSG;

namespace Feofun.Advertisment.Providers
{
    public class CheatAdsProvider : IAdsProvider
    {
        public bool IsInitialized => true;
        
        public void Init()
        {
            
        }

        public bool IsRewardAdsReady()
        {
            return true;
        }

        public IPromise<AdsResult> ShowRewardedAds()
        {
            return Promise<AdsResult>.Resolved(AdsResult.CreateSuccess());
        }

        public IPromise<AdsResult> ShowInterstitialAds()
        {
            return Promise<AdsResult>.Resolved(AdsResult.CreateSuccess());
        }
    }
}