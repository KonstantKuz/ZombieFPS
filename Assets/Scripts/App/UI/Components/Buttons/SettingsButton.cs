using Feofun.UI.Dialog;
using UnityEngine;
using Zenject;
using SettingsDialog = App.UI.Dialogs.Settings.SettingsDialog;

namespace App.UI.Components.Buttons
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class SettingsButton : MonoBehaviour
    {
        [Inject] private DialogManager _dialogManager;
        
        private void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenSettings);
        }

        private void OnDestroy()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(OpenSettings);
        }

        private void OpenSettings()
        {
            _dialogManager.Show<SettingsDialog>();
        }
    }
}