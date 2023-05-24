using App.Enemy.Service;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.LevelInfo
{
    public class LevelInfoPresenter : MonoBehaviour
    {
        [SerializeField] private LevelInfoView _view;

        [Inject] private EnemySpawnService _enemySpawnService;

        private LevelInfoModel _model;

        private void OnEnable()
        {
            _model = new LevelInfoModel(_enemySpawnService);
            _view.Init(_model);
        }

        private void OnDisable()
        {
            _model?.Dispose();
          
        }
    }
}