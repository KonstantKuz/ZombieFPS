using App.Items.Data;
using App.Items.Service;
using App.UI.Dialogs.Character.Model;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.UI.Dialogs.Character.Model.Slots;
using App.UI.Dialogs.Character.View;
using App.UI.Dialogs.Character.View.Inventory;
using App.UI.Dialogs.Character.View.Slots;
using App.UI.Dialogs.ItemInfo;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace App.UI.Dialogs.Character
{
    public class CharacterDialog : BaseDialog
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private SlotsView _slotsView;
        [SerializeField] private ItemCursor _itemCursor;
        [SerializeField] private ScrollView _scrollView;
        [SerializeField] private UIBehaviour _rootUI;
        [SerializeField] private ActionButton _closeButton;

        [Inject] private ItemService _itemService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private Feofun.World.World _world;
        
        private CompositeDisposable _disposable;
        private CharacterScreenModel _model;
        private CharacterScreenItemActionHandler _itemActionHandler;
        private SlotsModel SlotsModel => _model.SlotsModel;


        public void OnEnable()
        {
            _world.Pause();
            InitModel(InventorySectionType.Weapon);
            _closeButton.Init(() => _dialogManager.Hide<CharacterDialog>());
        }
        
        private void InitModel(InventorySectionType sectionType)
        {
            Dispose();
            _disposable = new CompositeDisposable();

            _model = new CharacterScreenModel(sectionType, _itemService, SwitchInventorySection, OnItemClick,
                OnBeginItemDrag, OnEndItemDrag, OnContextMenuClick);
            
            _itemActionHandler = new CharacterScreenItemActionHandler(_itemService, 
                _itemCursor,
                _slotsView,
                _scrollView,
                _model,
                ShowItemInfoDialog,
                 ReloadScreen,
                _analytics);
            
            _itemService.InventoryItemsAsObservable.Subscribe(it => UpdateInventorySection()).AddTo(_disposable);
            _itemService.AnySlotsObservable.Subscribe(it => SlotsModel.UpdateSlots()).AddTo(_disposable);
            _rootUI.OnPointerClickAsObservable().Subscribe(it => _itemActionHandler.TryUnselectItem()).AddTo(_disposable);
            
            _inventoryView.Init(_model.InventoryModel);
            _slotsView.Init(SlotsModel);
        }
        
        private void ReloadScreen() => InitModel(_model.InventorySectionType);
        
        private void OnContextMenuClick(ContextMenuButtonType buttonType, Item item) => _itemActionHandler.OnContextMenuClick(buttonType, item);

        private void OnEndItemDrag(SlotId itemSlotId, ItemViewModel itemModel) => _itemActionHandler.OnEndItemDrag(itemSlotId, itemModel);

        private void OnBeginItemDrag(SlotId itemSlotId) => _itemActionHandler.OnBeginItemDrag(itemSlotId);

        private void OnItemClick(ItemViewModel selectedItem) => _itemActionHandler.OnItemClick(selectedItem);

        private void ShowItemInfoDialog(Item item) => _dialogManager.Show<ItemInfoDialog, Item>(item);

        private void UpdateInventorySection() => _model.UpdateInventorySection(_model.InventorySectionType);
        
        private void SwitchInventorySection(InventorySectionType sectionType) => _model.UpdateInventorySection(sectionType);

        public void OnDisable()
        {
            Dispose();
            _world.UnPause();
        }

        private void Dispose()
        {
            _model = null;
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}