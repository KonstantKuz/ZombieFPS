using System;
using UnityEngine;

namespace App.Advertisment.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AdsConfig", fileName = "AdsConfig")]
    public class AdsConfig : ScriptableObject
    {
        [SerializeField]
        private string _androidAppKey = "19520a9c5";
        [SerializeField]
        private string _iOSAppKey = "195df3c45";
        [SerializeField]
        private bool _usingFacebook = true;   
        [SerializeField]
        private bool _enableAdapterDebug = true;
        
        public bool EnableAdapterDebug => _enableAdapterDebug;
        
        public string GetAppKey()
        {
#if UNITY_ANDROID
            return _androidAppKey;
#elif UNITY_IOS
            return _iOSAppKey;
#endif
            return String.Empty;
        }
        
    }
}