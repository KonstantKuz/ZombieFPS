using System.Collections;
using System.Linq;
using App.Items.Data;
using App.Items.Service;
using App.Tutorial.Service;
using App.Tutorial.WaitConditions;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.UI.Dialogs.Character.View.Inventory;
using App.UI.Dialogs.Character.View.Inventory.ContextMenu;
using App.UI.Screen.World;
using App.UI.Tutorial;
using Feofun.UI.Message;
using Feofun.UI.Tutorial;
using SuperMaxim.Messaging;
using UniRx;
using Zenject;

namespace App.Tutorial.Scenario
{
    public class EquipWeaponScenario: TutorialScenario
    {
        private const string INVENTORY_BUTTON_ID = "InventoryButton";

        [Inject] private IMessenger _messenger;
        [Inject] private TutorialService _tutorialService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private ItemService _itemService;
        
        private TutorialUiTools UiTools => _tutorialService.UiTools;

        public override void Init()
        {
            _messenger.Subscribe<ScreenSwitchMessage>(OnScreenSwitch);
        }

        private void OnScreenSwitch(ScreenSwitchMessage obj)
        {
            if(IsCompleted) return;
            if(obj.ScreenName != WorldScreen.ID.ToString()) return;
            
            var areSlotsFull = _itemService.GetSlotsBySection(InventorySectionType.Weapon).All(it => !it.IsEmpty);
            var unEquippedWeapon = _itemService.InventoryItems.FirstOrDefault(it => it.Type == ItemType.Weapon);
            if (areSlotsFull && unEquippedWeapon != null)
            {
                StartCoroutine(TutorialCoroutine(unEquippedWeapon));
            }
        }

        private IEnumerator TutorialCoroutine(Item unEquippedWeapon)
        {
            IsActive = true;
            var reporter = new TutorialStepReporter(_analytics, ScenarioId);

            reporter.Report();
            PointUiElement(INVENTORY_BUTTON_ID, HandDirection.Right);
            yield return new WaitForTutorialElementClicked(INVENTORY_BUTTON_ID);
            
            reporter.Report();
            var newWeaponId = unEquippedWeapon.Id;
            var weaponTutorialId = InventoryItemView.GetTutorialName(newWeaponId);
            yield return new WaitForTutorialElementActivated(weaponTutorialId);

            PointUiElement(weaponTutorialId);
            yield return new WaitForTutorialElementClicked(weaponTutorialId);
            
            reporter.Report();
            var equippedButtonId = ContextMenuButton.GetTutorialId(ContextMenuButtonType.Equip);
            yield return new WaitForTutorialElementActivated(equippedButtonId);
            
            PointUiElement(equippedButtonId, HandDirection.Down);
            yield return new WaitForTutorialElementClicked(equippedButtonId);
    
            reporter.Report();
            var weaponId = GetFirstEquippedWeapon();
            var firstEquipped =  InventoryItemView.GetTutorialName(weaponId);
            PointUiElement(firstEquipped, HandDirection.Left);
            yield return WaitForNewWeaponEquipped(newWeaponId);

            reporter.Report();
            CompleteScenario();

            UiTools.ElementHighlighter.Clear();
            UiTools.TutorialHand.Hide();
            IsActive = false;
        }

        private string GetFirstEquippedWeapon()
        {
            return _itemService.GetSlotsBySection(InventorySectionType.Weapon).First().ItemId;
        }

        private IEnumerator WaitForNewWeaponEquipped(string newWeaponId)
        {
            yield return _itemService.GetSlotKitAsObservable(ItemType.Weapon)
                .Where(it => it.SlotItems.Values.Contains(newWeaponId))
                .First()
                .ToYieldInstruction();
        }
        
        private void PointUiElement(string elementId, HandDirection direction = HandDirection.Up)
        {
            var uiElement = TutorialUiElementObserver.Get(elementId);
            UiTools.ElementHighlighter.Set(uiElement);
            UiTools.TutorialHand.ShowOnElement(uiElement.PointerPosition, direction);
        }
    }
}