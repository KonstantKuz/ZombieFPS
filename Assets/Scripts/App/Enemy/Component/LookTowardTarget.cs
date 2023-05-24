using System;
using Feofun.Components;
using Feofun.Extension;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace App.Enemy.Component
{
    public class LookTowardTarget : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [SerializeField] private float _lookIKWeight = 0.75f;
        
        private Animator _animator;
        private Unit.Unit _owner;
        private IDisposable _disposable;

        private void Awake()
        {
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _disposable = _animator.OnAnimatorIKAsObservable().Subscribe(OnIK);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

        public void Init(Unit.Unit unit)
        {
            _owner = unit;
        }

        private void OnIK(int layerIndex)
        {
            if (_owner == null) return;
            var target = _owner.TargetProvider?.Target;
            if (target == null) return;
            _animator.SetLookAtPosition(target.Center.position);
            _animator.SetLookAtWeight(_lookIKWeight);
        }
    }
}