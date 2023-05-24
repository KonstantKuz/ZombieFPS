using Logger.Extension;
using UnityEngine;

namespace Feofun.Util.FpsCount.Test
{
    public class TestDeltaTimeOnAppPause : MonoBehaviour
    {
        private int _logFramesCount;
        private bool _logFramesInUpdate;
    
        private void OnApplicationFocus(bool hasFocus)
        {
            this.Logger().Debug($"[OnApplicationFocus] : hasFocus {hasFocus} deltaTime {Time.deltaTime}, unscaled delta time {Time.unscaledDeltaTime}");
        }

        private void OnApplicationPause(bool pauseStatus)
        { 
            this.Logger().Debug($"[OnApplicationPause] : pauseStatus {pauseStatus} deltaTime {Time.deltaTime}, unscaled delta time {Time.unscaledDeltaTime}");
            _logFramesCount = 10;
            _logFramesInUpdate = !pauseStatus;
        }

        private void OnApplicationQuit()
        {
            this.Logger().Debug($"[OnApplicationQuit] : deltaTime {Time.deltaTime}, unscaled delta time {Time.unscaledDeltaTime}");
        }

        private void Update()
        {
            if(!_logFramesInUpdate || _logFramesCount < 0) return;
            _logFramesCount--;
            this.Logger().Debug($"[UpdateAfterApplicationPause] : deltaTime {Time.deltaTime}, unscaled delta time {Time.unscaledDeltaTime}");
        }
    }
}
