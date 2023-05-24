using System;
using System.Linq;
using App.Advertisment.Data;
using App.Booster.Config;
using App.Booster.Messages;
using App.Booster.Service;
using App.Tutorial.Service;
using App.Enemy.Service;
using App.Level.Service;
using App.Player.Progress.Service;
using App.Session.Messages;
using App.UI.Components;
using Feofun.Extension;
using Feofun.Localization;
using SuperMaxim.Messaging;
using UniRx;
using Zenject;

namespace App.UI.Screen.World.Booster.Model
{
    public class BoosterButtonModel
    {
        private const string LOCALIZABLE_TEXT_POSTFIX = "Button";
        private const int ACTIVATION_MIN_LEVEL = 2;
        
        private readonly ReactiveProperty<bool> _isButtonEnabled = new ReactiveProperty<bool>(true);
        private readonly CompositeDisposable _disposable;
        private readonly TimerModel _timer;

        public readonly string RewardedPlacementId;
        public IObservable<bool> Interactive { get; }
        public IObservable<bool> IsButtonEnabled => _isButtonEnabled;
        public IObservable<bool> IsTimerEnabled => _timer.IsTicking;
        public IObservable<string> TimerText => _timer.Seconds
            .Select(it => TimeSpan.FromSeconds(it))
            .Select(it => it.ToString(@"mm\:ss"));
        public LocalizableText Text { get; }

        public readonly Action<bool> OnRewardedShown;

        public BoosterButtonModel(BoosterService boosterService, 
                TutorialService tutorialService, 
                EnemySpawnService enemySpawnService, 
                PlayerProgressService playerProgressService,
                IMessenger messenger, 
                BoosterId boosterId, 
                Action<bool> onRewardedShown)
        {
            _disposable = new CompositeDisposable();
            _timer = new TimerModel();
            OnRewardedShown = onRewardedShown;
            Text = LocalizableText.Create(GetLocalizableKey(boosterId));
            RewardedPlacementId = RewardedAdsType.Booster.CreateRewardedPlacementId(boosterId.ToString());
            Interactive = boosterService.GetBoosterStateAsObservable(boosterId)
                .Select(it => !boosterService.IsBoosterActivated(boosterId));

            SetupTimer(boosterService, boosterId);
            SetupEnabledState(enemySpawnService, tutorialService, playerProgressService, messenger);
        }

        private void SetupTimer(BoosterService boosterService, BoosterId boosterId)
        {
            boosterService.GetBoosterStateAsObservable(boosterId)
                .Where(it => it != null)
                .Subscribe(UpdateTimerState).AddTo(_disposable);
        }
        
        private void UpdateTimerState(BoosterStateChangedData boosterState)
        {
            switch (boosterState.State)
            {
                case BoosterState.Started:
                    _timer.StartTimer((int) (boosterState.Booster.Duration - boosterState.Booster.ElapsedTime));
                    break;
                case BoosterState.Stopped:
                    _timer.Dispose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupEnabledState(EnemySpawnService enemySpawnService, 
            TutorialService tutorialService,
            PlayerProgressService playerProgressService,
            IMessenger messenger)
        {
            if (playerProgressService.Progress.PlayerLevel < ACTIVATION_MIN_LEVEL)
            {
                _isButtonEnabled.Value = false;
                return;
            }
            
            var whenLevelStarted = messenger.GetObservable<SessionStartMessage>()
                .Select(it => true);
            var whenRoundFinished = enemySpawnService.RoundInfoAsObservable
                .Where(it => it?.Result != null)
                .Select(it => true);
            var whenTimerFinished = _timer.IsTicking.SkipLatestValueOnSubscribe()
                .Where(it => !it)
                .Select(it => false);
            var whenAnyTutorialActive = tutorialService.IsAnyScenarioActiveAsObservable
                .Select(it => !it);
            var enabledState = whenLevelStarted.Merge(whenRoundFinished, whenTimerFinished, whenAnyTutorialActive);
            enabledState.Subscribe(it => _isButtonEnabled.Value = it).AddTo(_disposable);
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _disposable?.Dispose();
        }

        private string GetLocalizableKey(BoosterId boosterId) =>
            boosterId + nameof(BoosterId) + LOCALIZABLE_TEXT_POSTFIX;
    }
}