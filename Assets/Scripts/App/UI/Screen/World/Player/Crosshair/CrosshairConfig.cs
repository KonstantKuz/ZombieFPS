using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.UI.Screen.World.Player.Crosshair
{
    [CreateAssetMenu(fileName = "CrosshairConfig", menuName = "ScriptableObjects/CrosshairConfig")]
    public class CrosshairConfig : ScriptableObject
    {
        [SerializeField] private float _minIdleOffset;
        [SerializeField] private float _maxIdleOffset;
        [SerializeField] private float _shootOffset;
        [SerializeField] private SerializableDictionary<string, float> _durationMap;

        public float MinIdleOffset => _minIdleOffset;
        public float MaxIdleOffset => _maxIdleOffset;
        public float ShootOffset => _shootOffset;
        public SerializableDictionary<string, float> DurationMap => _durationMap;
    }
}