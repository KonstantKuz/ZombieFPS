using Feofun.UI.Dialog;
using UnityEngine;
using Zenject;


namespace App.UI.Components.Buttons
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class DialogOpenButton : MonoBehaviour
    {
        [SerializeField]
        private string _dialogName;

        [Inject] private DialogManager _dialogManager;
        
        private void Awake() => GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Open);

        private void OnDestroy() => GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(Open);

        private void Open() => _dialogManager.Show(_dialogName);
    }
}