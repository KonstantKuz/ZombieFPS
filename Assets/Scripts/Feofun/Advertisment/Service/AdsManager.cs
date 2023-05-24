using System;
using Feofun.Advertisment.Data;
using Feofun.Advertisment.Providers;
using Feofun.Analytics;
using Logger.Extension;
using RSG;
using Zenject;

namespace Feofun.Advertisment.Service
{
    public class AdsManager
    {
        private IAdsProvider _adsProvider;
        
        [Inject]
        private IAnalytics _analytics;
        public IAdsProvider AdsProvider 
        { 
            get => _adsProvider;
            set
            {
                _adsProvider = value;
                this.Logger().Info($"IAdsProvider changed, new AdsProvider is {_adsProvider.GetType().Name}");
                _adsProvider.Init();
            }
        }

        public bool IsInitialized => _adsProvider != null && _adsProvider.IsInitialized;
        
        public IPromise<AdsResult> ShowInterstitialAds() => AdsProvider.ShowInterstitialAds();

        public bool IsRewardAdsReady() => AdsProvider.IsRewardAdsReady();

        public IPromise<AdsResult> ShowRewardedAds(string placementId)
        {
            _analytics.ReportAdRewardedRequested(placementId);
            if (_adsProvider == null) {
                throw new NullReferenceException("IAdsProvider is null");
            }
            return _adsProvider.ShowRewardedAds()
                .Then(adResult => {
                    LogAdsResult(adResult);
                    SendAnalytics(adResult, placementId);
                    return adResult;
                });
        }
        
        private void SendAnalytics(AdsResult adResult, string placementId)
        {
            if (adResult.Success) {
                _analytics.ReportAdRewardedShown(placementId);
            }else {
                _analytics.ReportAdRewardedNotShown(placementId);
            }
        }
        
        private void LogAdsResult(AdsResult adsResult)
        {
            this.Logger().Trace($"Reward ads success is {adsResult.Success}");
            if(adsResult.Success) return;
            var adFail = adsResult.AdFail;
            if (adFail.Status is AdFailStatus.ShowFault or AdFailStatus.RepeatedCall) {
                this.Logger().Error(adFail.ToString());
                return;
            }
            this.Logger().Warn(adFail.ToString());
        }
    }
}