using UnityEngine;

namespace Feofun.Util.FpsCount.Test
{
    public class TestFpsSetter : MonoBehaviour
    {
        [SerializeField] private int _regularFps = 60;
        [SerializeField] private int _lowFps = 10;
        [SerializeField] private int _freezeFps = 5;

        [SerializeField] private KeyCode _fpsSwitchKey = KeyCode.Space;
        [SerializeField] private KeyCode _freezeCallKey = KeyCode.LeftShift;
        private bool _low;

        private void Update()
        {
            var fps = _low ? _lowFps : _regularFps;
            
            if (Input.GetKeyDown(_fpsSwitchKey))
            {
                _low = !_low;
            }

            if (Input.GetKeyDown(_freezeCallKey))
            {
                fps = _freezeFps;
            }
            
            Application.targetFrameRate = fps;
        }
    }
}
