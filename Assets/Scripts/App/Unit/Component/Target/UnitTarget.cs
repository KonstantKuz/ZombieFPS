using System;
using App.Unit.Component.Layering;
using App.Unit.Service;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace App.Unit.Component.Target
{
    public class UnitTarget : MonoBehaviour, ITarget, IInitializable<Unit>
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _center;

        private Unit _owner;
        
        [Inject] private TargetService _targetService;
        
        public bool IsValid { get; private set; }
        public LayerMask Layer => _owner.LayerMaskProvider.Layer;
        public Transform Root => _root;
        public Transform Center => _center;
        public event Action OnTargetInvalid;
        
        public void Init(Unit owner)
        {
            _owner = owner;
            IsValid = true;
            _targetService.Add(this);
            owner.OnDeath += OnInvalid;
        }

        private void OnDisable()
        {
            OnInvalid(_owner);
        }

        private void OnInvalid(Unit owner)
        {
            if(!IsValid) return;
            
            _owner.OnDeath -= OnInvalid;
            IsValid = false;
            _targetService.Remove(this);
            OnTargetInvalid?.Invoke();
        }
    }
}