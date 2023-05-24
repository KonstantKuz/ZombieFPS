using System.Collections.Generic;
using Feofun.Extension;
using Feofun.World.Service;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Feofun.World.Factory.ObjectFactory.Factories
{
    public class ObjectInstancingFactory : IObjectFactory
    {
        private readonly HashSet<GameObject> _createdObjects = new HashSet<GameObject>();
        
        private CompositeDisposable _disposable;
        
        [Inject]
        private World _world;     
        [Inject]
        private ObjectResourceService _objectResourceService;
        [Inject]
        private DiContainer _container;

        private CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();

        public T Create<T>(string objectId, Transform container = null)
        {
            var prefab = _objectResourceService.GetPrefab(objectId);
            return Create<T>(prefab.GameObject, container);
        }
        public T Create<T>(GameObject prefab, Transform container = null)
        {
            return CreateObject(prefab, container).RequireComponent<T>();
        }
        
        public T Create<T>(Component prefabComponent, Transform container = null)
        {
            return Create<T>(prefabComponent.gameObject, container);
        }
        
        public void Destroy(GameObject instance) => GameObject.Destroy(instance);
        
        private GameObject CreateObject(GameObject prefab, [CanBeNull] Transform container = null)
        {
            var parentContainer = container == null ? _world.SpawnContainer.transform : container.transform;
            var createdGameObject = _container.InstantiatePrefab(prefab, parentContainer);
            _createdObjects.Add(createdGameObject);
            createdGameObject.OnDestroyAsObservable().Subscribe((o) => RemoveObject(createdGameObject)).AddTo(Disposable);
            return createdGameObject;
        }
        public void DestroyAllObjects()
        {
            _createdObjects.ForEach(Destroy);
            Dispose();
            _createdObjects.Clear();
        }
        private void RemoveObject(GameObject obj) => _createdObjects.Remove(obj);
        
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}