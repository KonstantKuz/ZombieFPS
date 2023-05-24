using System.Collections.Generic;
using UnityEngine;

namespace App.Config.Configs
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PromoRecordSettings", fileName = "PromoRecordSettings")]
    public class PromoRecordSettings : ScriptableObject
    {
        [SerializeField] private bool _hudElementsEnabled = true;
        [SerializeField] private int _maxTextureSize = 2048;
        [SerializeField] private List<string> _targetFolders = new List<string> { "Assets/ThirdParty/SurrounDead/Textures" };
        
        public bool HudElementsEnabled => _hudElementsEnabled;
        public int MaxTextureSize => _maxTextureSize;
        public List<string> TargetFolders => _targetFolders;
    }
}