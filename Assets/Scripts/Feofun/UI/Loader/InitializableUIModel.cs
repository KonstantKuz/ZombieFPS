using Feofun.UI.Components;
using UnityEngine;

namespace Feofun.UI.Loader
{
    public class InitializableUIModel<TUIObject, TParam> : UIModel<TUIObject> where TUIObject : MonoBehaviour, IUiInitializable<TParam>
    {
        public TParam InitParameter { get; private set; }
        public static InitializableUIModel<TUIObject, TParam> Create(TParam initParameter)
        {
            return new InitializableUIModel<TUIObject, TParam>()
            {
                InitParameter = initParameter,
            };
        }
        public new InitializableUIModel<TUIObject, TParam> SetContainer(Transform container)
        {
            base.SetContainer(container);
            return this;
        }  
        public new InitializableUIModel<TUIObject, TParam> SetPrefab(TUIObject prefab)
        {
            base.SetPrefab(prefab);
            return this;
        }
        public new InitializableUIModel<TUIObject, TParam> SetPath(string path)
        {
            base.SetPath(path);
            return this;
        }
    }
}