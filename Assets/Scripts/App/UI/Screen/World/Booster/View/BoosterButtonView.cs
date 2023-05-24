using System.Collections.Generic;
using App.UI.Screen.World.Booster.Model;
using Feofun.Extension;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using TMPro;
using UniRx;
using UnityEngine;

namespace App.UI.Screen.World.Booster.View
{
    public class BoosterButtonView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rootContainer;
        [SerializeField]
        private TextMeshProUGUI _timerText;
        [SerializeField]
        private TextMeshProLocalization _text;
        [SerializeField]
        private List<GameObject> _interactiveStateObjects;

        private CompositeDisposable _disposable;
        private ButtonWithRewardAds _button;
        private ButtonWithRewardAds Button => _button ??= gameObject.RequireComponentInChildren<ButtonWithRewardAds>();
        public void Init(BoosterButtonModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            Button.Init(model.OnRewardedShown);
            _text.SetTextFormatted(model.Text);
            _button.AdsPlacementId = model.RewardedPlacementId;
            model.Interactive.Subscribe(OnStateUpdated).AddTo(_disposable);
            model.IsButtonEnabled.Subscribe(SetActive).AddTo(_disposable);
            model.IsTimerEnabled.Subscribe(SwitchLabelToTimer).AddTo(_disposable);
            model.TimerText.Subscribe(OnTimerUpdate).AddTo(_disposable);
        }

        private void OnStateUpdated(bool interactive)
        {
            _interactiveStateObjects.ForEach(it => it.SetActive(interactive));
            Button.Interactable = interactive;
        }

        private void SetActive(bool value)
        {
            _rootContainer.SetActive(value);
        }

        private void SwitchLabelToTimer(bool value)
        {
            _text.gameObject.SetActive(!value);
            _timerText.gameObject.SetActive(value);
        }

        private void OnTimerUpdate(string time)
        {
            _timerText.SetText(time);
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}