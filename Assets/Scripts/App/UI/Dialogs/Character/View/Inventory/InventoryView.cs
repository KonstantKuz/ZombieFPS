using System;
using System.Collections.Generic;
using System.Linq;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Extension;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.Character.View.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemView _itemPrefab;
        [SerializeField]
        private Transform _itemRoot;
        
                
        private Dictionary<InventorySectionType, InventorySectionButton> _buttons;
        private CompositeDisposable _disposable;
        
        [Inject] private DiContainer _container;
        
        
        private Dictionary<InventorySectionType, InventorySectionButton> Buttons =>
            _buttons ??= GetComponentsInChildren<InventorySectionButton>().ToDictionary(button => button.Type, 
                button=> button);

        public void Init(IReactiveProperty<InventoryModel> model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            model.Subscribe(UpdateModel).AddTo(_disposable);
        }
        private void UpdateModel(InventoryModel model)
        {
            RemoveAllCreatedObjects();
            UpdateItems(model.Items);
            UpdateButtons(model.Buttons);
        }
        private void UpdateItems(List<ItemViewModel> items)
        {
            foreach (var itemModel in items)
            {
                var itemView = _container.InstantiatePrefabForComponent<InventoryItemView>(_itemPrefab, _itemRoot);
                itemView.Init(itemModel);
            }
        }   
        
        private void UpdateButtons(List<InventorySectionButtonModel> buttons)
        {
            foreach (var buttonModel in buttons)
            {
                if (!Buttons.ContainsKey(buttonModel.Type)) {
                    throw new NullReferenceException(
                        $"Button not found by InventorySectionType:= {buttonModel.Type}");
                }
                Buttons[buttonModel.Type].Init(buttonModel);
            }
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