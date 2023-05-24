using System;
using App.Enemy.Dismemberment.Component.BodyMember.BehaviourStrategy;
using App.Enemy.Dismemberment.Model;
using App.Enemy.Model;
using App.Unit.Component.Health;
using App.Unit.Extension;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Profiling;

namespace App.Enemy.Dismemberment.Component.BodyMember
{
    [RequireComponent(typeof(BodyMemberHealth))]
    public class BodyMemberBehaviour : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [SerializeField]
        private BodyMemberType _bodyMemberType;

        private BodyMemberHealth _health;
    
        private BodyMemberModel _memberModel;
        private IBodyMemberBehaviour _behaviour;

        public BodyMemberType BodyMemberType => _bodyMemberType;
        private bool ShouldDetach => _health.Current.Value <= _memberModel.ExtraHealth && _behaviour.ShouldDetach;

        public bool IsDetached => !_behaviour.ShouldDetach;

        public event Action<BodyMemberBehaviour> OnMemberDetached;
        public event Action OnMemberDestroyed;

        public void Init(Unit.Unit unit)
        {
            _memberModel = unit.RequireModel<EnemyUnitModel>().BodyModel.GetMember(_bodyMemberType);
            _health.Init(_memberModel.MainHealth + _memberModel.ExtraHealth);
            _behaviour.Init(unit);
        }
        
        private void Awake()
        {
            _health = GetComponent<BodyMemberHealth>();
            _behaviour = IBodyMemberBehaviour.CreateBehaviour(gameObject, BodyMemberType);
            
            _health.OnDamageTaken += OnDamageTaken;
            _health.OnZeroHealth += OnZeroHealth;
        }
        
        public void Dismember()
        {
            _behaviour.Dismember();
            OnMemberDetached?.Invoke(this);
        }
        
        private void OnZeroHealth()
        {
            _behaviour.Kill();
            OnMemberDestroyed?.Invoke();
        }
        
        private void OnDamageTaken(DamageInfo obj)
        {
            if (!ShouldDetach) return;
            Profiler.BeginSample("[BodyMemberBehaviour] Dismember the part");
            Dismember();
            Profiler.EndSample();
     
        }
        private void OnDestroy()
        {
            _health.OnDamageTaken -= OnDamageTaken;
            _health.OnZeroHealth -= OnZeroHealth;
        }
    }
}