using UnityEngine;
using Zenject;

namespace App.Player.Config
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllerConfig", fileName = "PlayerControllerConfig")]
    public class PlayerControllerConfig : ScriptableObject
    {
        [Inject] private PlayerControllerLoadableConfig _loadableConfig;
        
        public float GestureSensitivityCoefficient
        {
            get => _loadableConfig.SensitivityCoefficient;
            set => _loadableConfig.SensitivityCoefficient = value;
        }

        [SerializeField] public float MouseSensitivityCoefficient = 100f;
        
        [SerializeField] public float MaxPitchAngle = 60f;
        
        [SerializeField] public float TimeoutBeforeRotateToTarget = 2f;
        [SerializeField] public bool  IsRotatingToTargetEnabled;
        [SerializeField] public float RotationSpeedToTarget = 2f;
        
        
        [SerializeField] public MouseButton RotationEnabledSwitchingMouseButton = MouseButton.Right;
        [SerializeField] public KeyCode RunningKeyCode = KeyCode.LeftShift;      
        [SerializeField] public KeyCode WeaponReloadingKeyCode = KeyCode.R;   
      
        [SerializeField] public KeyCode[] SlotsKeyCodes = {  
            
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4
            
        };
    }
}