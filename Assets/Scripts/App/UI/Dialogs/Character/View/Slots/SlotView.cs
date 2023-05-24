using App.Items.Data;
using App.UI.Dialogs.Character.Model.Slots;
using App.UI.Dialogs.Character.View.Inventory;
using App.UI.Dialogs.Character.View.Inventory.ContextMenu;
using Feofun.Extension;
using Feofun.UI.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.Character.View.Slots
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField]
        private SlotId _slotId;
        [SerializeField]
        private TextMeshProLocalization _text; 
        [SerializeField]
        private SlotEmptyItemView _emptyItem;
        [SerializeField]
        private InventoryItemView _itemPrefab;
        [SerializeField] 
        private Transform _itemRoot;
        [SerializeField] 
        private ContextMenuType _contextMenuType = ContextMenuType.Lower;

        private Animator _animator;
        
        [Inject]
        private DiContainer _container;
        
        private CompositeDisposable _disposable;

        public SlotId SlotId => _slotId;

        private Animator Animator => _animator ??= GetComponent<Animator>();
        
        public void Init(SlotViewModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _emptyItem.Init(model.EmptyItemModel);
            _text.SetTextFormatted(model.Text);
            
            model.ItemModel.Subscribe(it => UpdateItemModel(model)).AddTo(_disposable);
            model.State.Subscribe(UpdateState).AddTo(_disposable);
        }

        private void UpdateState(SlotViewState state)
        {
            foreach (var stateName in EnumExt.Values<SlotViewState>()) {
                Animator.SetBool(Animator.StringToHash(stateName.ToString()), false);
            }
            Animator.SetBool(Animator.StringToHash(state.ToString()), true);
            
            
        }

        public InventoryItemView GetItemView() => _itemRoot.gameObject.RequireComponentInChildren<InventoryItemView>();
        
        private void UpdateItemModel(SlotViewModel model)
        {
            RemoveAllCreatedObjects();
            _emptyItem.gameObject.SetActive(model.IsEmptyView);
            if(model.IsEmptyView) return;
            var itemView = _container.InstantiatePrefabForComponent<InventoryItemView>(_itemPrefab, _itemRoot);
            itemView.Init(model.ItemModel.Value, _contextMenuType);
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        { 
            _disposable?.Dispose();
            _disposable = null;
            RemoveAllCreatedObjects();
        }

        private void RemoveAllCreatedObjects() => _itemRoot.DestroyAllChildren();
    }
}