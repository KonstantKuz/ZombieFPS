using System;
using App.Unit.Component.Layering;
using App.Unit.Service;
using Feofun.Extension;
using UnityEngine;
using Zenject;

namespace App.Unit.Component.Target
{
    public class DummyTarget : MonoBehaviour, ITarget
    {
        private LayerMaskProvider _layerMaskProvider;
        
        [Inject] private TargetService _targetService;

        private LayerMaskProvider LayerMaskProvider => _layerMaskProvider ??= gameObject.RequireComponent<LayerMaskProvider>();
        public bool IsValid { get; private set; }
        public LayerMask Layer => LayerMaskProvider.Layer;
        public Transform Root => transform;
        public Transform Center => transform;
        public event Action OnTargetInvalid;

        private void OnEnable()
        {
            IsValid = true;
            _targetService.Add(this);
        }

        private void OnDisable()
        {
            IsValid = false;
            _targetService.Remove(this);
            OnTargetInvalid?.Invoke();
        }
    }
}