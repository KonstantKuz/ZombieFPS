using App.Items.Config;
using App.Items.Data;
using App.Items.Service;
using App.Player.Model;
using App.UI.Dialogs.Character.View.Inventory;
using Feofun.Modifiers.Service;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.ItemInfo
{
    public class ItemInfoDialog : BaseDialog, IUiInitializable<Item>
    {
        [SerializeField] private InventoryItemView _itemView;
        [SerializeField] private ParameterListView _parameterListView;
        [SerializeField] private ActionButton _nextItemButton;
        [SerializeField] private ActionButton _previousItemButton;
        [SerializeField] private ActionButton _closeButton;

        private CompositeDisposable _disposable;
        
        [Inject] private ItemModifiersConfigCollection _modifiersCollection;
        [Inject] private PlayerModelBuilder _playerModelBuilder;
        [Inject] private ModifierFactory _modifierFactory;
        [Inject] private ItemService _itemService;

        public void Init(Item item)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            var model = new ItemInfoDialogModel(item, _playerModelBuilder, _modifiersCollection, _modifierFactory, _itemService);
            model.ItemModel.Subscribe(it => _itemView.Init(it)).AddTo(_disposable);
            model.ParametersList.Subscribe(it => _parameterListView.Init(it)).AddTo(_disposable);
            _nextItemButton.Init(model.SelectNextItem);
            _previousItemButton.Init(model.SelectPreviousItem);
            _closeButton.Init(HideDialog);
        }
        
    

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
