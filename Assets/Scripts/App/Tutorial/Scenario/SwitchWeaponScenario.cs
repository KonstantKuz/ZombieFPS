using System.Collections;
using App.BattlePass.Service;
using App.Player.Progress.Service;
using App.Tutorial.Service;
using App.Tutorial.WaitConditions;
using App.UI.Screen.World;
using App.UI.Screen.World.Player.RuntimeInventory.View;
using App.UI.Tutorial;
using App.Weapon.Service;
using Feofun.UI.Message;
using Feofun.UI.Tutorial;
using SuperMaxim.Messaging;
using UniRx;
using Zenject;

namespace App.Tutorial.Scenario
{
    public class SwitchWeaponScenario: TutorialScenario
    {
        private const int SECOND_LEVEL = 2;
        private const string LEVEL_START_BUTTON = "LevelStartButton";
        
        [Inject] private IMessenger _messenger;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private TutorialService _tutorialService;
        [Inject] private WeaponService _weaponService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private BattlePassService _battlePassService;
        private TutorialStepReporter _reporter;

        private TutorialUiTools UiTools => _tutorialService.UiTools;
        private string PreviousLevelRewardItemId => _battlePassService.FindRewardConfigByLevel(SECOND_LEVEL - 1).RewardId;

        public override void Init()
        {
            _messenger.Subscribe<ScreenSwitchMessage>(OnScreenSwitched);
        }

        private void OnScreenSwitched(ScreenSwitchMessage msg)
        {
            if (IsCompleted) return;
            if (msg.ScreenName != WorldScreen.ID.ToString()) return;
            if (_playerProgressService.Progress.PlayerLevel != SECOND_LEVEL) return;

            StartCoroutine(TutorialCoroutine());
        }

        private IEnumerator TutorialCoroutine()
        {
            IsActive = true;
            _reporter = new TutorialStepReporter(_analytics, ScenarioId);
            yield return RunSwitchWeaponStep();
            yield return RunPressStartStep();
            CompleteScenario();
            _reporter.Report();
            IsActive = false;
            _reporter = null;
        }
        
        private IEnumerator RunSwitchWeaponStep()
        {
            _reporter.Report();

            var itemId = PreviousLevelRewardItemId;
            var rifleButtonId = InventoryItemView.GetTutorialId(itemId);
            yield return new WaitForTutorialElementActivated(rifleButtonId);
            
            var uiElement = TutorialUiElementObserver.Get(rifleButtonId);
            UiTools.ElementHighlighter.Set(uiElement);
            UiTools.TutorialHand.ShowOnElement(uiElement.PointerPosition, HandDirection.Down);

            yield return _weaponService.ActiveWeaponId
                .Where(it => it == itemId)
                .First()
                .ToYieldInstruction();
        }

        private IEnumerator RunPressStartStep()
        {
            _reporter.Report();
            
            var uiElement = TutorialUiElementObserver.Get(LEVEL_START_BUTTON);
            UiTools.ElementHighlighter.Set(uiElement);
            UiTools.TutorialHand.ShowOnElement(uiElement.PointerPosition, HandDirection.Down);
            yield return new WaitForTutorialElementClicked(LEVEL_START_BUTTON);

            UiTools.ElementHighlighter.Clear();
            UiTools.TutorialHand.Hide();
        }
    }
}