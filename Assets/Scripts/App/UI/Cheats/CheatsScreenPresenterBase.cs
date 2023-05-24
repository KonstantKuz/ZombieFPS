﻿using System.Linq;
using App.ABTest;
using App.Cheats;
using Feofun.Cheats;
using Feofun.Extension;
using Feofun.UI.Components.Button;
using Feofun.UI.Components.Dropdown;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Cheats
{
    public class CheatsScreenPresenterBase : MonoBehaviour
    {
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _hideButton;

        [SerializeField] private ActionToggle _toggleConsoleButton;
        [SerializeField] private ActionToggle _toggleFPSButton;
        [SerializeField] private ActionToggle _toggleAdsButton;   
        [SerializeField] private ActionToggle _toggleABTestButton; 

        [SerializeField] private ActionButton _resetProgressButton;

        [SerializeField] private InputField _inputField; 
        [SerializeField] private ActionButton _setLanguage;

        [SerializeField] private ActionButton _setRussianLanguage;
        [SerializeField] private ActionButton _setEnglishLanguage;

        [SerializeField] private ActionButton _testLogButton;     
        
        [SerializeField] private ActionButton _analyticsTestButton;
      
        
        [SerializeField] private DropdownWithButtonView _abTestDropdown;

        [Inject] protected CheatsManager _cheatsManager;
        [Inject] private CheatsActivator _cheatsActivator;

        private void OnEnable()
        {
            Init();
        }

        protected virtual void Init()
        {
            _closeButton.Init(HideCheatsScreen);
            _hideButton.Init(DisableCheats);

            _toggleConsoleButton.Init(_cheatsManager.IsConsoleEnabled, value => { _cheatsManager.IsConsoleEnabled = value; });
            _toggleFPSButton.Init(_cheatsManager.IsFPSMonitorEnabled, value => { _cheatsManager.IsFPSMonitorEnabled = value; });
            _toggleAdsButton.Init(_cheatsManager.IsAdsCheatEnabled, value => _cheatsManager.IsAdsCheatEnabled = value);
            _toggleABTestButton.Init(_cheatsManager.IsABTestCheatEnabled, value => _cheatsManager.IsABTestCheatEnabled = value);

            _resetProgressButton.Init(_cheatsManager.ResetProgress);

            _setLanguage.Init(() => _cheatsManager.SetLanguage(_inputField.text));
            _setEnglishLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.English.ToString()));
            _setRussianLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.Russian.ToString()));
            _testLogButton.Init(_cheatsManager.LogTestMessage);
            _analyticsTestButton.Init(_cheatsManager.ReportAnalyticsTestEvent);
            
            _abTestDropdown.Init(EnumExt.Values<ABTestVariantId>().Select(it => it.ToCamelCase()).ToList(), _cheatsManager.SetCheatAbTest);
        }

        private void DisableCheats()
        {
            _cheatsActivator.ShowOpenCheatButton(false);
            _cheatsActivator.EnableCheats(false);
            HideCheatsScreen();
        }

        private void HideCheatsScreen() => _cheatsActivator.ShowCheatsScreen(false);
    }
}