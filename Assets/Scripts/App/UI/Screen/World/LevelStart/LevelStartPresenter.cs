using System;
using App.Player.Progress.Service;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.LevelStart
{
    public class LevelStartPresenter : MonoBehaviour
    {
        [SerializeField]
        private LevelStartView _startView;
        
        [Inject] private PlayerProgressService _playerProgress;
        public void Init(Action onStart)
        {
            Disable();
            if (_playerProgress.Progress.GameCount < 1) {
                onStart.Invoke();
                return;
            }
            _startView.Init(onStart, _playerProgress.Progress.PlayerLevel);
        }
        public void Disable() => _startView.SetActive(false);
        
    }
}