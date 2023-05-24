using System;
using Feofun.Extension;
using Feofun.UI.Components;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Feofun.UI.Loader
{
    public class UILoader
    {
        [Inject]
        private DiContainer _container;

        public TUIObject Load<TUIObject>(UIModel<TUIObject> model) where TUIObject : MonoBehaviour
        {
            if (model.UIPath == null) {
                throw new NullReferenceException("Path to prefab is null");
            }
            var loadedPrefab = Resources.Load<TUIObject>(model.UIPath);
            model.SetPrefab(loadedPrefab);
            return Instantiate(model);
        }
        
        public TUIObject Load<TUIObject, TParam>(InitializableUIModel<TUIObject, TParam> model) where TUIObject : MonoBehaviour, IUiInitializable<TParam>
        {
            var uiObject = Load<TUIObject>(model);
            uiObject.Init(model.InitParameter);
            return uiObject;
        }

        public TUIObject Instantiate<TUIObject>(UIModel<TUIObject> model) where TUIObject : MonoBehaviour
        {
            if (model.UIPrefab == null) {
                throw new NullReferenceException("Instance prefab is null");
            }
            var uiObject = InstantiatePrefab<TUIObject>(model.UIPrefab, model.UIContainer);
            return uiObject;
        }
        
        public TUIObject Instantiate<TUIObject, TParam>(InitializableUIModel<TUIObject, TParam> model) where TUIObject : MonoBehaviour, IUiInitializable<TParam>
        {
            var uiObject = Instantiate<TUIObject>(model);
            uiObject.Init(model.InitParameter);
            return uiObject;
        }
      
        private TUIObject InstantiatePrefab<TUIObject>(Object prefab, Transform position) where TUIObject : MonoBehaviour
        {
            return _container.InstantiatePrefab(prefab, position).RequireComponent<TUIObject>();
        }
    }
}