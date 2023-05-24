using System;
using App.Settings;
using UniRx;

namespace App.UI.Dialogs.Settings
{
    public class SettingsDialogModel
    {
        public readonly IconSwitchButtonModel SoundButtonModel;
        public readonly IconSwitchButtonModel VibrationButtonModel;

        private readonly BoolReactiveProperty _soundState;
        private readonly BoolReactiveProperty _vibrationState;
        private readonly Action _updateSettingsAction;
        private readonly Action _onEnableVibration;

        public SettingsDialogModel(SettingsData settingsData, 
            Action updateSettingsAction,
            Action onEnableVibration)
        {
            _updateSettingsAction = updateSettingsAction;
            _onEnableVibration = onEnableVibration;
            _soundState = new BoolReactiveProperty(settingsData.SoundEnabled);
            _vibrationState = new BoolReactiveProperty(settingsData.VibrationEnabled);
            
            SoundButtonModel = new IconSwitchButtonModel
            {
                State = _soundState,
                OnPressed = SwitchSoundState
            };
            VibrationButtonModel = new IconSwitchButtonModel
            {
                State = _vibrationState,
                OnPressed = SwitchVibrationState
            };
        }

        private void SwitchSoundState()
        {
            _soundState.Value = !_soundState.Value;
            _updateSettingsAction.Invoke();
        }

        private void SwitchVibrationState()
        {
            _vibrationState.Value = !_vibrationState.Value;
            _updateSettingsAction.Invoke();
            if (_vibrationState.Value)
            {
                _onEnableVibration.Invoke();
            }
        }
    }
}