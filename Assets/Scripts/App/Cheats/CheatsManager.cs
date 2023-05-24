using System;
using App.Cheats.Data;
using App.Cheats.Repository;
using App.Player.Config;
using Feofun.ABTest;
using Feofun.ABTest.Providers;
using Feofun.Advertisment.Providers;
using Feofun.Advertisment.Service;
using Feofun.Localization.Service;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace App.Cheats
{
    public class CheatsManager : MonoBehaviour, IABTestCheatManager
    {
        private const string TEST_LOG_MESSAGE = "Test log message";
        
        private readonly CheatRepository _repository = new();
        
        [SerializeField] private GameObject _fpsMonitor;
        [SerializeField] private GameObject _debugConsole;
        
        
        [Inject] protected LocalizationService _localizationService;
        [Inject] protected Analytics.Analytics _analytics;     
        [Inject] protected Feofun.ABTest.ABTest _abTest;
        [Inject(Optional = true)] private AdsManager _adsManager;    
        [Inject] protected DiContainer _diContainer;   
        
        [Inject] private PlayerControllerConfig _playerControllerConfig;

        public string GestureSensitivityCoefficient
        {
            get => _playerControllerConfig.GestureSensitivityCoefficient.ToString();
            set => _playerControllerConfig.GestureSensitivityCoefficient = Int32.Parse(value);
        }

        protected CheatSettings Settings => _repository.Get() ?? new CheatSettings();
        
        public void Init()
        {
#if DEBUG_CONSOLE_ENABLED
            IsConsoleEnabled = true;
#endif
#if FPS_MONITOR_ENABLED
            IsFPSMonitorEnabled = true;
#endif
            _debugConsole.SetActive(IsConsoleEnabled); 
            _fpsMonitor.SetActive(IsFPSMonitorEnabled);
        }
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        public void ReportAnalyticsTestEvent()
        {
            _analytics.ReportTest();
        }
        
        
        public void LogTestMessage()
        {
            var logger = this.Logger();
            logger.Trace(TEST_LOG_MESSAGE);
            logger.Debug(TEST_LOG_MESSAGE);      
            logger.Info(TEST_LOG_MESSAGE);
            logger.Warn(TEST_LOG_MESSAGE);     
            logger.Error(TEST_LOG_MESSAGE);
        }

        public void SetLanguage(string language)
        {
            _localizationService.SetLanguageOverride(language);
        }
        private void UpdateSettings(Action<CheatSettings> updateFunc)
        {
            var settings = Settings;
            updateFunc?.Invoke(settings);
            _repository.Set(settings);
        }
        public bool IsConsoleEnabled
        {
            get => Settings.ConsoleEnabled;
            set
            {
                UpdateSettings(settings => { settings.ConsoleEnabled = value; });
                _debugConsole.SetActive(value);
            }
        }    
        public bool IsFPSMonitorEnabled
        {
            get => Settings.FPSMonitorEnabled;
            set
            {
                UpdateSettings(settings => { settings.FPSMonitorEnabled = value; });
                _fpsMonitor.SetActive(value);
            }
        }
        
        public void SetCheatAbTest(string variantId)
        {
            OverrideABTestProvider.SetVariantId(variantId);
            _abTest.Reload();
        }
        
        public bool IsAdsCheatEnabled  {
            get => _adsManager?.AdsProvider is CheatAdsProvider;
            set 
            {
                if (_adsManager == null) return;
                _adsManager.AdsProvider = value ? new CheatAdsProvider() : _diContainer.Resolve<IAdsProvider>(); 
            }
        } 
        
        public bool IsABTestCheatEnabled
        {
            get => Settings.ABTestCheatEnabled;
            set
            {
                UpdateSettings(settings => { settings.ABTestCheatEnabled = value; });
                _abTest.Reload();
            }
        }
        
    }
}

