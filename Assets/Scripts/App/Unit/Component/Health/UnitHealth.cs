using App.Unit.Model;
using Feofun.Components;
using UnityEngine;

namespace App.Unit.Component.Health
{
    public class UnitHealth : Health, IInitializable<Unit>, IUpdatableComponent, IHealthOwner
    {
        private IHealthModel _model;
        
        public float Max => _model.MaxHealth;
        
        public void Init(Unit owner)
        {
            _model = owner.Model.HealthModel;
            base.Init(Max);
        }
        
        public void OnTick()
        {
            if (!IsAlive) return;
            Regenerate();
        }
        

        private void Regenerate()
        {
            if (Current.Value >= _model.MaxHealth) return;
            var newValue= Mathf.Min(_model.MaxHealth, Current.Value + _model.Regeneration * Time.deltaTime);
            SetHealth(newValue);
        }
    }
}