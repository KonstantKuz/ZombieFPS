using App.Vibration;
using UnityEngine;
using Zenject;

namespace App.Settings
{
    public class SettingsService
    {
        private readonly SettingsRepository _repository = new();

        public SettingsData Data => _repository.Get() ?? new SettingsData();

        [Inject] private VibrationManager _vibrationManager;

        public void Init()
        {
            Set(Data);
        }

        public void Set(SettingsData data)
        {
            AudioListener.volume = data.SoundEnabled ? 1f : 0f;
            _vibrationManager.SetVibrationEnabled(data.VibrationEnabled);
            _repository.Set(data);
        }
    }
}