using Feofun.Components;
using UnityEngine;

namespace App.Enemy.Component.Destroy
{
    [RequireComponent(typeof(Unit.Unit))]
    [RequireComponent(typeof(AutoDestroyByTimeoutWhenNotVisible))]
    public class DeadEnemyRemoval : MonoBehaviour, IInitializable<Unit.Unit>
    {
        private Unit.Unit _unit;
        private AutoDestroyByTimeoutWhenNotVisible _autoDestroy;

        private void Awake() => _autoDestroy = GetComponent<AutoDestroyByTimeoutWhenNotVisible>();

        public void Init(Unit.Unit unit)
        {
            _unit = unit;
            _unit.OnDeath += OnDeath;
        }
        
        private void OnDisable()
        {
            if (_unit != null) {
                _unit.OnDeath -= OnDeath;
            }
        }

        private void OnDeath(Unit.Unit _)
        {
            _autoDestroy.StartTimeout();
        }


    }
}