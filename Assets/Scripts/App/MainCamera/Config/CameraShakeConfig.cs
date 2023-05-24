using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace App.MainCamera.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CameraShakeConfig", fileName = "CameraShakeConfig")]
    public class CameraShakeConfig : ScriptableObject
    {
        [SerializeField] private List<CameraShakeLevel> _cameraShakeLevels;
        
        [CanBeNull]
        public CameraShakeLevel FindShakeConfig(string eventName)
        {
            var shakeType = _cameraShakeLevels
                    .Where(it => it.EventNames.Contains(eventName))
                    .ToList()
                    .FirstOrDefault();
            
            if (shakeType == null)
            {
                this.Logger().Warn($"Can't find shake type for event := {eventName}");
                return null;
            }

            return shakeType;
        }
    }
}
