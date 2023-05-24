using System;
using App.Advertisment.Extension;
using App.UI.Dialogs.NoAdsDialog;
using Feofun.Advertisment.Service;
using Feofun.UI.Dialog;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Feofun.UI.Components.Button
{
    [RequireComponent(typeof(ActionButton))]
    public class ButtonWithRewardAds : MonoBehaviour
    {
        [SerializeField]
        private string _adsPlacementId;
        
        [Inject] private AdsManager _adsManager;
        [Inject] private DialogManager _dialogManager;
        
        private ActionButton _button;
        private Action<bool> _adsCallback;
        private bool? _lastInteractable;
        private bool _isLock;
        
        private UnityEngine.UI.Button Button => ActionButton.Button;
        private ActionButton ActionButton => _button ??= GetComponent<ActionButton>();
        
        public string AdsPlacementId
        {
            get => _adsPlacementId;
            set => _adsPlacementId = value;
        }
        
        public bool Interactable
        {
            get => Button.interactable;
            set
            {
                _lastInteractable = value;
                if (!_isLock) {
                    Button.interactable = _lastInteractable.Value;
                }
            }
        }
        
        public void Init(Action<bool> adsCallback)
        {
            _adsCallback = adsCallback;
            ActionButton.Init(OnClick);
        }
        
        private void OnClick()
        {
            _lastInteractable ??= Button.interactable;
            Button.interactable = false;
            _isLock = true;
            Assert.IsTrue(!string.IsNullOrEmpty(AdsPlacementId), $"AdsPlacementId is null or empty, buttonName:= {name}");
            _adsManager.ShowRewardedAds(AdsPlacementId)
                .Done(adsResult => {
                    if (adsResult.ShouldShowNoAdsNotification()) {
                        _dialogManager.Show<NoAdsDialog>();
                    }
                    _isLock = false;
                    Interactable = _lastInteractable.Value;
                    _adsCallback?.Invoke(adsResult.Success);
                });
        }
    }
}