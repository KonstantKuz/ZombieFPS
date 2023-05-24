using System;
using App.Booster.Config;
using App.Booster.Service;
using App.UI.Screen.World.Booster.Model;
using App.UI.Screen.World.Booster.View;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Booster
{
    public class BoosterButtonPresenter : MonoBehaviour
    {
        [SerializeField]
        private BoosterId _boosterId;  
        [SerializeField]
        private BoosterButtonView _view;

        [Inject] private DiContainer _container;
        [Inject] private BoosterService _boosterService; 

        private BoosterButtonModel _model;
        
        private void OnEnable()
        {
            _model = _container.Instantiate<BoosterButtonModel>(new object[]{_boosterId, (Action<bool>) OnRewardedShown});
            _view.Init(_model);
        }   
        private void OnDisable()
        {
            _model?.Dispose();
            _model = null;
        }

        private void OnRewardedShown(bool success)
        {
            if (_boosterService.IsBoosterActivated(_boosterId)) {
                this.Logger().Error($"Booster is already active, id:= {_boosterId}");
                return;
            }
            if (success) {
                _boosterService.Start(_boosterId);
            }
        }
    }
}