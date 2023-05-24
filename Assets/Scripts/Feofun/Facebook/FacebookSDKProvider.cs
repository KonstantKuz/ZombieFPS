using System;
using Facebook.Unity;
using Logger.Extension;
using UnityEngine;

namespace Feofun.Facebook
{
    public class FacebookSDKProvider
    {
        private bool _isInitializationStarted;
        public bool IsInitialized { get; private set; }
        
        private Action<bool> _onInitializationCompleted;
        
        public void Init(Action<bool> onCompleted)
        {
            if (IsInitialized) {
                onCompleted?.Invoke(IsInitialized);
                return;
            }
            _onInitializationCompleted += onCompleted;
            if (_isInitializationStarted) {
                return;
            }
            _isInitializationStarted = true;
            this.Logger().Info("Starting initializing Facebook SDK");
            
            if (!FB.IsInitialized) { 
                FB.Init(InitCallback, OnAppVisibilityChange);
            } else {
                ActivateFB();
            }
        }
        
        private void InitCallback()
        {
            if (FB.IsInitialized) {
                ActivateFB();
                return;
            }  
            this.Logger().Info("Failed to Initialize the Facebook SDK");
            CompleteInit();
        }

        private void ActivateFB()
        {
            FB.Mobile.SetAdvertiserTrackingEnabled(true);
            FB.ActivateApp();
            IsInitialized = true;
            this.Logger().Info("Facebook SDK is Initialized");
            CompleteInit();
        }

        private void CompleteInit()
        {
            _isInitializationStarted = false;
            _onInitializationCompleted?.Invoke(IsInitialized);
            _onInitializationCompleted = null;
        }

        private void OnAppVisibilityChange(bool isVisible)
        {
            Time.timeScale = !isVisible ? 0 : 1;
        }
    }
}