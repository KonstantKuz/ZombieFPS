using Feofun.Config;
using UnityEngine.Profiling;
using Zenject;

namespace App.Vibration
{
    public class VibrationManager
    {
        [Inject]
        private ConfigCollection<VibrationType, VibrationConfig> _config;

        private bool _enabled = true;
        
        public VibrationManager()
        {
            global::Vibration.Init();
        }

        public void Vibrate(VibrationType type)
        {
            if (!_enabled) return;
            Profiler.BeginSample("Vibrate");
            global::Vibration.Vibrate((long)(1000 * _config.Get(type).Time));
            Profiler.EndSample();
        }

        public void SetVibrationEnabled(bool enabled)
        {
            _enabled = enabled;
            if (!_enabled)
            {
                global::Vibration.Cancel();
            }
        }
    }
}