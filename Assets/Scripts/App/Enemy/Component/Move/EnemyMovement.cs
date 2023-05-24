using System;
using App.Enemy.Model;
using App.Extension;
using App.Unit.Component;
using App.Unit.Component.Death;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace App.Enemy.Component.Move
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemySound))]
    public class EnemyMovement : MonoBehaviour, 
        IInitializable<Unit.Unit>, 
        IMessageListener<UnitDeathComponentMessage>,
        IMovementController
    {
        [SerializeField]
        private float _rotationSpeed = 3f;
        
        [CanBeNull]
        private MoveAnimationWrapper _moveAnimationWrapper;
        private NavMeshAgent _agent;
        private EnemySound _sound;

        private NavMeshAgent Agent
        {
            get
            {
                if (_agent == null) {
                    _agent = gameObject.RequireComponent<NavMeshAgent>();
                }
                if (gameObject.activeSelf && !_agent.enabled) {
                    _agent.enabled = true;
                }
                return _agent;
            }
        }

        public bool IsStopped 
        {
            get => !Agent.isOnNavMesh || Agent.isStopped;
            set
            {
                SetIsStopped(value);
                UpdateAnimation();
                Sound.SetWalkSound(!value);
            }
        }
        private EnemySound Sound => _sound ??= GetComponent<EnemySound>();
        
        private void Awake()
        {
            var animator = gameObject.GetComponentInChildren<Animator>();
            if (animator != null) {
                _moveAnimationWrapper = new MoveAnimationWrapper(animator); 
            }
        }

        public void Init(Unit.Unit owner)
        {
            Agent.enabled = true;
            var model = (EnemyUnitModel) owner.Model;
            SetMoveSpeed(model.MoveSpeed);
        }

        public void SetMoveSpeed(float speed)
        {
            Agent.speed = speed;
        }

        public void MoveTo(Vector3 destination)
        {
            SetDestination(destination);
        }
        public void LookAt(Vector3 target)
        {
            var lookDirection = (target - Agent.transform.position).XZ();
            var lookRotation = Quaternion.LookRotation(lookDirection);
            Agent.transform.rotation = Quaternion.Slerp(Agent.transform.rotation, lookRotation, 
                Time.deltaTime * _rotationSpeed);
        }
        public void RandomizeWalkAnimation()
        {
            _moveAnimationWrapper?.SetRandomTimeOfCurrentState();
        }
        private void UpdateAnimation()
        {
            if (IsStopped) {
                _moveAnimationWrapper?.PlayIdleSmooth();
            } else {
                _moveAnimationWrapper?.PlayMoveForwardSmooth();
            }
        }
        private void SetIsStopped(bool isStopped)
        {
            if (!Agent.isOnNavMesh && !isStopped) {
                throw new Exception("cannot start to move agent while it is not on nav mesh");
            }
            if (!Agent.isOnNavMesh || Agent.isStopped == isStopped) {
                return;
            }
            Agent.isStopped = isStopped;
        }
        private void SetDestination(Vector3 destination)
        {
            if (!Agent.isOnNavMesh) {
                this.Logger().Warn("SetDestination can only be called on an active agent that has been placed on a NavMesh.");
                return;
            }
            Agent.destination = destination;
        }
        
        private void OnDisable() => Agent.enabled = false;

        public void OnMessage(UnitDeathComponentMessage msg)
        {
            SetIsStopped(true);
            Agent.enabled = false;
        }

        public void SetPosition(Vector3 pos)
        {
            Agent.PlaceOnNavMesh(pos);
        }
    }
}