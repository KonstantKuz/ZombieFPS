using System;
using App.Enemy.Service;
using Feofun.UI.Components;
using UniRx;
using UnityEngine;

namespace App.UI.Screen.World.LevelInfo
{
    public class LevelInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProLocalization _roundText;
        [SerializeField] private GameObject _rountTimerRoot;
        [SerializeField] private TextMeshProLocalization _roundTimerText;
        [SerializeField] private GameObject _enemiesLeftRoot;
        [SerializeField] private TextMeshProLocalization _enemiesLeftText;
        
        private CompositeDisposable _disposable;
        private IDisposable _roundDisposable;

        public void Init(LevelInfoModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            model.ViewEnabled.Subscribe(SetActive).AddTo(_disposable);
            model.RoundInfo.Where(it => it != null).Subscribe(OnRoundStart).AddTo(_disposable);
            model.TimerEnabled.Subscribe(SetTimerEnabled).AddTo(_disposable);
            model.TimerSeconds.Subscribe(UpdateTimer).AddTo(_disposable);
        }

        private void OnRoundStart(RoundInfo roundInfo)
        {
            SetTextFormatted(_roundText, $"{roundInfo.RoundNumber}/{roundInfo.RoundsCount}");
            DisposeRound();
            _roundDisposable = roundInfo.EnemiesLeft.Subscribe(it => SetTextFormatted(_enemiesLeftText, it.ToString()));
        }

        private void SetTextFormatted(TextMeshProLocalization text, string value)
        {
            text.SetTextFormatted(text.LocalizationId, value);
        }

        private void SetTimerEnabled(bool value)
        {
            _rountTimerRoot.SetActive(value);
            _enemiesLeftRoot.SetActive(!value); 
        }

        private void UpdateTimer(int time)
        {
            SetTextFormatted(_roundTimerText, time.ToString());
        }

        private void DisposeRound()
        {
            _roundDisposable?.Dispose();
            _roundDisposable = null;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            DisposeRound();
            SetActive(false);
        }

        private void SetActive(bool enabled) => gameObject.SetActive(enabled);
    }
}
