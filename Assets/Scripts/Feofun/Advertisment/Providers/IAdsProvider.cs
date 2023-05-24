using Feofun.Advertisment.Data;
using RSG;

namespace Feofun.Advertisment.Providers
{
    public interface IAdsProvider
    {
        bool IsInitialized { get; }
        void Init();
        bool IsRewardAdsReady();
        IPromise<AdsResult> ShowRewardedAds();
        IPromise<AdsResult> ShowInterstitialAds();
    }
}