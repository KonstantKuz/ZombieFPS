using System;
using App.Tutorial.Service;
using Feofun.UI.Tutorial;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.LevelStart
{
    public class ShowNotificationHandByTimer : MonoBehaviour
    {
        [SerializeField]
        private float _appearDelay;

        [SerializeField] 
        private float _showTime;

        [SerializeField] 
        private Transform _target;
        
        [Inject] 
        private TutorialService _tutorialService;
        
        private IDisposable _disposable;
        private bool _isHandShown;
        private void OnEnable()
        {
            StartDelayTimer();
        }

        private void StartDelayTimer()
        {
            _disposable?.Dispose();
            _disposable = Observable.Timer(TimeSpan.FromSeconds(_appearDelay)).Subscribe(Show);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            if (_isHandShown)
            {
                _tutorialService.UiTools.TutorialHand.Hide();
            }
        }

        private void Show(long _)
        {
            _disposable?.Dispose();

            if (_tutorialService.IsAnyScenarioIsActive)
            {
                StartDelayTimer();
                return;
            }
            
            _disposable = Observable.Timer(TimeSpan.FromSeconds(_showTime)).Subscribe(Hide);
            _tutorialService.UiTools.TutorialHand.ShowOnElement(_target, HandDirection.Down);
            _isHandShown = true;
        }

        private void Hide(long _)
        {
            _isHandShown = false;
            _tutorialService.UiTools.TutorialHand.Hide();
            StartDelayTimer();
        }
    }
}