using UnityEngine;

namespace App.MiniMap.Config
{
    [CreateAssetMenu( menuName = "ScriptableObjects/MiniMapConfig", fileName = "MiniMapConfig")]
    public class MiniMapConfig : ScriptableObject
    {
        [SerializeField] private float _mapScale;

        public float MapScale => _mapScale;
    }
}