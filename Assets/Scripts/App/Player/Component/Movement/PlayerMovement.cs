using System;
using App.Extension;
using App.Player.Component.StateMachine;
using App.Player.Model;
using App.Unit.Component;
using App.Unit.Extension;
using EasyButtons;
using Feofun.Components;
using Feofun.Extension;
using Logger.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace App.Player.Component.Movement
{
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovement : MonoBehaviour, 
        IInitializable<Unit.Unit>, 
        IUpdatableComponent,
        IMovementController
    {
        private const float MAX_PLACE_ON_NAVMESH_DISTANCE = 100f;
        private PlayerStateMachine _playerStateMachine;
        private PlayerUnitModel _model;
        private Vector3 _previousPosition;
        private NavMeshAgent _agent;

        private PlayerState CurrentState => _playerStateMachine.CurrentStateName.Value;

        public float MoveSpeed => _model.MoveSpeed * (CurrentState == PlayerState.Running ? _model.RunningSpeedFactor : 1f);

        public bool IsMoving { get; private set; }

        public float WalkingMoveSpeed => _model.MoveSpeed;

        private NavMeshAgent Agent => _agent ??= GetComponent<NavMeshAgent>();


        public void Init(Unit.Unit owner)
        {
            _model = owner.RequireModel<PlayerUnitModel>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            PlacePlayerOnNavMesh();
        }

        [Button]
        private void PlacePlayerOnNavMesh()
        {
            if (NavMesh.SamplePosition(transform.position, out var hit, MAX_PLACE_ON_NAVMESH_DISTANCE, Agent.areaMask))
            {
                transform.position = hit.position;
            }
            else
            {
                this.Logger().Error("Failed to place player on navmesh");
            }
        }

        public void Move(Vector3 direction)
        {
            if (CurrentState == PlayerState.Dead) return;
            
            var desiredMove = transform.forward * direction.z + transform.right * direction.x;

            Agent.Move(desiredMove * MoveSpeed);
        }
        
        public void OnTick()
        {
            var position = transform.position;
            var movedDistance = Math.Abs(Vector3.Distance(position, _previousPosition));
            IsMoving = !movedDistance.IsZero();
            _previousPosition = position;
        }

        public void SetPosition(Vector3 pos)
        {
            Agent.PlaceOnNavMesh(pos);
        }
    }
}