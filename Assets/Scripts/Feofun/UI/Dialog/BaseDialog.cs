using UnityEngine;
using Zenject;

namespace Feofun.UI.Dialog
{
    public abstract class BaseDialog : MonoBehaviour
    {
        [Inject] 
        protected DialogManager _dialogManager;
        
        public void SetActive(bool value) => gameObject.SetActive(value);
        
        public void HideDialog() => _dialogManager.Hide(GetType());
    }
}