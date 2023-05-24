using System;
using System.Collections.Generic;
using System.Linq;
using App.Player.Model;
using App.Unit.Component;
using App.Unit.Component.Death;
using App.Unit.Component.Health;
using App.Unit.Component.Layering;
using App.Unit.Component.Message;
using App.Unit.Component.Target;
using App.Unit.Message;
using App.Unit.Model;
using App.Unit.Service;
using Dreamteck;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using Feofun.Core.Update;
using Feofun.Extension;
using Feofun.World.Model;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace App.Unit
{
    [RequireComponent(typeof(Destroyer))]
    [RequireComponent(typeof(UnitHealth))]
    public class Unit : WorldObject, IMovementLockable, ITimeStoppable, IUpdatablesList
    {
        private IUnitDeath _death;
        private readonly MultiMessagePublisher _messagePublisher = new();
        private ISet<IUpdatableComponent> _updatables;
        private int _lockCount;
        private bool _isActive;
        private LayerMaskProvider _layerMaskProvider;
        private IMovementController _movementController;
        private Destroyer _destroyer;

        [Inject] private UpdateManager _updateManager;
        [Inject] private TargetService _targetService;
        [Inject] private UnitService _unitService;
        [Inject] private IMessenger _messenger;
        
        public IUnitModel Model { get; private set; }

        public LayerMaskProvider LayerMaskProvider =>
            _layerMaskProvider ??= gameObject.RequireComponent<LayerMaskProvider>();
        
        [CanBeNull]
        public ITargetProvider TargetProvider { get; set; }
        public ITarget SelfTarget { get; private set; }
        public UnitHealth Health { get; private set; }
        public event Action<Unit> OnDeath;
        public bool IsActive
        {
            get => _isActive;
            private set {
                if (_isActive == value) {
                    return;
                }
                _isActive = value;
                _messagePublisher.Publish(new UnitStateChangedComponentMessage(_isActive));
            }
        }

        public bool IsDied => !Health.IsAlive;

        private void Awake()
        {
            _death = gameObject.RequireComponent<IUnitDeath>();
            SelfTarget = gameObject.RequireComponent<ITarget>();
            Health = GetComponent<UnitHealth>();
            _destroyer = GetComponent<Destroyer>();
            _messagePublisher.CollectListeners<UnitStateChangedComponentMessage>(gameObject, true);
            _messagePublisher.CollectListeners<UnitDeathComponentMessage>(gameObject, true);
            _messagePublisher.CollectListeners<TimeStopStateChangedComponentMessage>(gameObject, true);
            _updatables = GetComponentsInChildren<IUpdatableComponent>().ToHashSet();
            _movementController = GetComponentInChildren<IMovementController>();
        }

        public void Init(IUnitModel unitModel)
        {
            Model = unitModel;
            IsActive = true;
            InitComponents();
            Health.OnZeroHealth += OnZeroHealth;
            _updateManager.StartUpdate(UpdateComponents);
            _unitService.Add(this);
            _messenger.Publish(new UnitInitMessage(this));
        }

        public void AddUpdatables(ISet<IUpdatableComponent> updatables) => _updatables.UnionWith(updatables);
        public void RemoveUpdatables(ISet<IUpdatableComponent> updatables) => _updatables.ExceptWith(updatables);

        private void OnZeroHealth()
        {
            Health.OnZeroHealth -= OnZeroHealth;
            Kill();
        }

        private void Kill()
        {
            IsActive = false;
            Dispose();
            _messagePublisher.Publish(new UnitDeathComponentMessage());
            _death.PlayDeath();
            OnDeath?.Invoke(this);
            _messenger.Publish(new UnitDeadMessage(this));
        }
        public void SetNearestTargetProvider(float searchDistance)
        {
            TargetProvider = new NearestTargetProvider(_targetService, this, searchDistance);
        }
        
        public void Lock()
        {
            _lockCount++;
            IsActive = false;
        }

        public void UnLock()
        {
            if (_lockCount > 0) {
                _lockCount--;
            }
            if (_lockCount <= 0) {
                IsActive = true;
            }
        }

        public void StopTime()
        {
            Lock();
            _messagePublisher.Publish(new TimeStopStateChangedComponentMessage {IsStopped = true});
        }

        public void StartTime()
        {
            _messagePublisher.Publish(new TimeStopStateChangedComponentMessage {IsStopped = false});
            UnLock();
        }
        
        public void SetPosition(Vector3 position)
        {
            _movementController?.SetPosition(position);
        }

        public void Destroy() => _destroyer.Destroy();

        private void InitComponents()
        {
            GetComponentsInChildren<IInitializable<Unit>>(true).ForEach(it => it.Init(this));
            _updatables = GetComponentsInChildren<IUpdatableComponent>(true).ToHashSet();
        }
        private void UpdateComponents()
        {
            if (!IsActive) {
                return;
            } 
            foreach (var updatable in _updatables) {
                updatable.OnTick();
            }
        }
        
        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _updateManager.StopUpdate(UpdateComponents);
            _unitService.Remove(this);
            Health.OnZeroHealth -= OnZeroHealth;
        }
    }
}