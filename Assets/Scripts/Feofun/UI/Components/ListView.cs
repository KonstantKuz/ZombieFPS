using System;
using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Feofun.UI.Components
{
    public class ListView : MonoBehaviour
    {
        [SerializeField] 
        private Transform _root;
        [SerializeField] 
        private GameObject _itemPrefab;

        private IDisposable _disposable;
        
        [Inject]
        private DiContainer _container;
        
        public void Init<T>(IReadOnlyReactiveProperty<List<T>> itemModels)
        {
            Dispose();
            _disposable = itemModels.Subscribe(UpdateItems);
        }

        private void UpdateItems<T>(IReadOnlyList<T> itemModels)
        {
            _root.DestroyAllChildren();
            itemModels.ForEach(itemModel =>
            {
                var itemView = _container.InstantiatePrefabForComponent<IUiInitializable<T>>(_itemPrefab, _root);
                itemView.Init(itemModel);
            });
            _root.gameObject.SetActive(itemModels.Count > 0);
        }

        private void Dispose()
        {
            _root.DestroyAllChildren();
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
