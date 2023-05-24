using App.Player.Progress.Service;
using App.Session.Messages;
using Feofun.UI.Components.Button;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace App.Tutorial.Scenario
{
    public class ControlTutorialScenario: TutorialScenario
    {
        private const int LEVEL = 1;

        [SerializeField] private GameObject _tutorialPopup;
        [SerializeField] private ActionButton _button;
        
        [Inject] private IMessenger _messenger;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private Feofun.World.World _world;
        public override void Init()
        {
            _messenger.Subscribe<SessionStartMessage>(OnSessionStarted);
            _button.Init(FinishScenario);
        }

        private void OnSessionStarted(SessionStartMessage msg)
        {
            if (IsCompleted) return;
            if (_playerProgressService.Progress.PlayerLevel != LEVEL) return;
            StartScenario();
        }

        private void StartScenario()
        {
            IsActive = true;
            _tutorialPopup.SetActive(true);
            _world.Pause();
            CompleteScenario();
        }

        private void FinishScenario()
        {
            _world.UnPause();
            _tutorialPopup.SetActive(false);
            IsActive = false;
        }
    }
}