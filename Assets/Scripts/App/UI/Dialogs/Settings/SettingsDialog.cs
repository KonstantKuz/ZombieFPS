using App.Settings;
using App.Vibration;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.Settings
{
    public class SettingsDialog : BaseDialog
    {
        [SerializeField] private IconSwitchButton _soundButton;
        [SerializeField] private IconSwitchButton _vibrationButton;
        [SerializeField] private ActionButton _closeButton;
        
        [Inject] private SettingsService _settingsService;
        [Inject] private Feofun.World.World _world;
        [Inject] private VibrationManager _vibrationManager;
        private SettingsDialogModel _model;

        private void OnEnable()
        {
            _model = new SettingsDialogModel(_settingsService.Data, UpdateSettings, Vibrate);
            _soundButton.Init(_model.SoundButtonModel);
            _vibrationButton.Init(_model.VibrationButtonModel);
            _closeButton.Init(HideDialog);
            _world.Pause();
        }

        private void OnDisable()
        {
            _world.UnPause();
        }
        
        private void UpdateSettings()
        {
            var data = _settingsService.Data;
            data.SoundEnabled = _model.SoundButtonModel.State.Value;
            data.VibrationEnabled = _model.VibrationButtonModel.State.Value;
            _settingsService.Set(data);
        }

        private void Vibrate()
        {
            _vibrationManager.Vibrate(VibrationType.Medium);
        }
    }
}