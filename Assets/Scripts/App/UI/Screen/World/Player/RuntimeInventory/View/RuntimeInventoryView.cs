using System.Collections.Generic;
using App.UI.Screen.World.Player.RuntimeInventory.Model;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player.RuntimeInventory.View
{
    public class RuntimeInventoryView : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemView _itemPrefab;       
        [SerializeField]
        private GameObject _emptyItemPrefab;
        [SerializeField]
        private Transform _root;

        private CompositeDisposable _disposable;
        
        [Inject] private DiContainer _container;

        public void Init(IEnumerable<IReactiveProperty<ItemViewModel>> items)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            items.ForEach(CreateItem);
        }
        private void CreateItem(IReactiveProperty<ItemViewModel> itemViewModel)
        {
            if (itemViewModel.Value.State.Value == ItemViewState.Empty) {
                Instantiate(_emptyItemPrefab, _root);
                return;
            }
            var itemView = _container.InstantiatePrefabForComponent<InventoryItemView>(_itemPrefab, _root);
            itemViewModel.Subscribe(itemView.Init).AddTo(_disposable);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            RemoveAllCreatedObjects();
        }
        private void RemoveAllCreatedObjects()
        {
            _root.DestroyAllChildren();
        }
    }
}