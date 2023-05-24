using System;
using App.Advertisment.Config;
using Feofun.Advertisment.Data;
using Feofun.Advertisment.Providers;
using Logger.Extension;
using Zenject;
using RSG;

namespace App.Advertisment
{
    public class IronSourceAdsProvider : IAdsProvider
    {
        [Inject]
        private AdsConfig _adsConfig;
        
        private Promise<AdsResult> _rewardedAdResultPromise;

        public bool IsInitialized { get; private set; }

        public void Init()
        {
            if (IsInitialized) {
                return;
            }
            InitIronSource();
        }

        private void InitIronSource()
        {
            this.Logger().Info("IronSource configuration is started");
            
            var appKey = _adsConfig.GetAppKey();
            if (appKey.Equals(string.Empty)) {
                this.Logger().Warn("IronSource cannot init without AppKey");
                return;
            }

            IronSourceEvents.onSdkInitializationCompletedEvent += OnIronSourceInitialized;
            
            if (_adsConfig.EnableAdapterDebug) {
                this.Logger().Trace("IronSource, GetAdvertiserId : " + IronSource.Agent.getAdvertiserId());
                IronSource.Agent.validateIntegration();
                IronSource.Agent.setAdaptersDebug(true);
            }
            
            this.Logger().Trace("IronSource calls init");
            IronSource.Agent.shouldTrackNetworkState(true);
            IronSource.Agent.init(appKey);
        }
        
        private void OnIronSourceInitialized()
        {
            this.Logger().Info("IronSource, initialization completed successfully");
            IronSourceEvents.onSdkInitializationCompletedEvent -= OnIronSourceInitialized;
            IsInitialized = true;
            InitializeRewardedAds();
        }
        
        private void InitializeRewardedAds()
        {
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        }
        
    
        public bool IsRewardAdsReady() => IsInitialized && IronSource.Agent.isRewardedVideoAvailable();

        public IPromise<AdsResult> ShowRewardedAds()
        { 
            if (!IsInitialized) {
                this.Logger().Trace("IronSource not initialized");
                return Promise<AdsResult>.Resolved(AdsResult.CreateFail("IronSource not initialized", AdFailStatus.NotInitialized));
            }
            if (_rewardedAdResultPromise != null) {
                this.Logger().Warn("ShowRewardedAds already called"); 
                return Promise<AdsResult>.Resolved(AdsResult.CreateFail("IronSource already called", 
                    AdFailStatus.RepeatedCall));
            }
            if (!IsRewardAdsReady()) {
                this.Logger().Trace("RewardedAd not ready");
                return Promise<AdsResult>.Resolved(AdsResult.CreateFail("RewardedAds not ready", AdFailStatus.NotAvailable));
            }
            _rewardedAdResultPromise = new Promise<AdsResult>();
            IronSource.Agent.showRewardedVideo();
            return _rewardedAdResultPromise;
        }

        public IPromise<AdsResult> ShowInterstitialAds()
        {
            throw new NotImplementedException();
        }
        
        private void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) 
        {
            this.Logger().Trace($"RewardedVideoOnAdAvailable, adInfo = {adInfo}");
        }
        private void RewardedVideoOnAdUnavailable()
        {
            this.Logger().Trace("RewardedVideoOnAdUnavailable");
        }
        private void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            this.Logger().Trace($"RewardedVideoOnAdOpenedEvent, adInfo = {adInfo}");
        }
        private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            this.Logger().Trace($"RewardedVideoOnAdClosedEvent, adInfo = {adInfo}");
        }
        private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            this.Logger().Trace($"RewardedVideoOnAdRewardedEvent, adInfo = {adInfo}");
            CallOnRewardedAdsSuccess();
        }
        private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            this.Logger().Error($"RewardedVideoOnAdShowFailedEvent, error = {error}, adInfo = {adInfo}");
            CallOnRewardedAdsFail($"Failed to display Rewarded Ads, error = {error}", AdFailStatus.ShowFault);
        }

        private void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            this.Logger().Trace($"RewardedVideoOnAdClickedEvent, adInfo = {adInfo}");
        }
        
        private void CallOnRewardedAdsFail(string message, AdFailStatus status)
        {
            _rewardedAdResultPromise?.Resolve(AdsResult.CreateFail(message, status));
            _rewardedAdResultPromise = null;
        }

        private void CallOnRewardedAdsSuccess()
        {
            _rewardedAdResultPromise?.Resolve(AdsResult.CreateSuccess());
            _rewardedAdResultPromise = null;
        }

    }
}