using System;
using App.Unit.Component.Health;
using UnityEngine;

namespace App.UI.Components.FrameEffects.Effects.HealthFrameEffect
{
    public class HealthInitializableFrameEffect : InitializableFrameEffect<IHealthOwner>
    {
        [SerializeField]
        private HealthFrameView _view;
        
        private IHealthOwner _healthOwner;
        public override void Init(IHealthOwner healthOwner)
        {
            _healthOwner = healthOwner;
        }
        public override void StartEffect(Action onComplete)
        {
            base.StartEffect(onComplete);
            var model = new HealthFrameModel(_healthOwner);
            _view.Init(model);
        }
    }
}