using JetBrains.Annotations;
using UnityEngine;

namespace Feofun.UI.Loader
{
    public class UIModel<TUIObject> where TUIObject : MonoBehaviour
    {
        public Transform UIContainer { get; private set; }
        [CanBeNull]
        public string UIPath { get; private set; }
        [CanBeNull]
        public TUIObject UIPrefab { get; private set; }
        
        public UIModel<TUIObject> SetContainer(Transform container)
        {
            UIContainer = container;
            return this;
        }  
        public UIModel<TUIObject> SetPrefab(TUIObject prefab)
        {
            UIPrefab = prefab;
            return this;
        }
        public UIModel<TUIObject> SetPath(string path)
        {
            UIPath = path;
            return this;
        }
    }
}