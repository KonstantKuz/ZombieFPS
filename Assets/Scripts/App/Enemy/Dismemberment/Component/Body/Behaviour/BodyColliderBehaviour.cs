using System;
using App.Unit.Component.Death;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    public class BodyColliderBehaviour : MonoBehaviour, IInitializable<Unit.Unit>, IMessageListener<UnitDeathComponentMessage>
    {
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private SerializableDictionary<BodyMoveMode, ColliderParams> _colliderParams;

        private BodyMoveBehaviour _bodyMoveBehaviour;

        private void Awake()
        {
            _bodyMoveBehaviour = gameObject.RequireComponentInParent<BodyMoveBehaviour>();
        }

        public void Init(Unit.Unit data)
        {
            var isInitialized = Enum.TryParse(_bodyMoveBehaviour.CurrentStateName, out BodyMoveMode _);
            if (isInitialized)
            {
                OnStateChanged(_bodyMoveBehaviour.CurrentStateName);
            }
            _bodyMoveBehaviour.OnStateChanged += OnStateChanged;
        }

        private void OnStateChanged(string stateName)
        {
            var state = Enum.Parse<BodyMoveMode>(stateName);
            if (!_colliderParams.ContainsKey(state)) 
            {
                throw new NullReferenceException($"Must set collider params for state := {state}");
            }
            
            _collider.center = _colliderParams[state].Center;
            _collider.direction = _colliderParams[state].Axis;
        }

        public void OnMessage(UnitDeathComponentMessage msg)
        {
            _bodyMoveBehaviour.OnStateChanged -= OnStateChanged;
            _collider.enabled = false;
        }

        private void OnDestroy()
        {
            _bodyMoveBehaviour.OnStateChanged -= OnStateChanged;
        }

        [Serializable]
        public class ColliderParams
        {
            public Vector3 Center;
            public int Axis;
        }
    }
}