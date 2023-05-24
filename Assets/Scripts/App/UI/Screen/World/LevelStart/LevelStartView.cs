using System;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using UnityEngine;

namespace App.UI.Screen.World.LevelStart
{
    public class LevelStartView : MonoBehaviour
    {
        [SerializeField] private ActionButton _playButton;  
        [SerializeField] private TextMeshProLocalization _localizableText;
        
        public void Init(Action onStart, int levelNumber)
        {
            SetActive(true);
            _playButton.Init(onStart);
            _localizableText.SetArgs(levelNumber);
        }
        
        public void SetActive(bool enabled) => gameObject.SetActive(enabled);
        
    }
}