using System;
using App.Unit.Component.Message;
using DG.Tweening;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace App.Unit.Component.DamageReaction
{
    [RequireComponent(typeof(Unit))]  
    [RequireComponent(typeof(NavMeshAgent))]
    public class ForceStrikeReaction : MonoBehaviour, IMessageListener<TimeStopStateChangedComponentMessage>
    {
        private readonly int _startFallingHash = Animator.StringToHash("StartFalling");      
        private readonly int _isFallingHash = Animator.StringToHash("IsFalling");
        
        [SerializeField] private ParticleSystem _groundhitEffect;
        [SerializeField] private float _effectPlayTime;
        [SerializeField] private Transform _effectPosition;
        [SerializeField] private TrailRenderer _trail;
        
        private Unit _owner;
        private NavMeshAgent _agent;
        private Action _damageCallback;
        private Sequence _sequence;
        private Animator _animator;
        
        private void Awake()
        {
            _owner = GetComponent<Unit>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
        }

        public static void TryExecuteOn(GameObject target, float height, float time, Action damageCallback)
        {
            if (!target.TryGetComponent(out ForceStrikeReaction explosionReaction)) { return; }
            explosionReaction.OnExplosionReact(height, time, damageCallback);
        }

        private void OnExplosionReact(float height, float time, Action damageCallback)
        {
            if (gameObject == null) return;
            if (_sequence != null) return;

            _animator.SetTrigger(_startFallingHash);
            _animator.SetBool(_isFallingHash, true);
            
            _sequence = DOTween.Sequence();
            _sequence.Append(transform
                .DOMoveY( transform.position.y + height, 0.5f * time)
                .SetEase(Ease.OutCubic));
            _sequence.AppendCallback(EnableTrail);
            _sequence.Append(transform.DOMoveY(transform.position.y, 0.5f * time)
                .SetEase(Ease.OutBounce));
            _sequence.Join(DOTween.Sequence().InsertCallback(_effectPlayTime * time, PlayHitVfx));
            _sequence.OnComplete(CompleteExplosionJump);

            _owner.Lock();
            _agent.enabled = false;

            _damageCallback = damageCallback;
        }
        private void EnableTrail()
        {
            _trail.Clear();
            _trail.gameObject.SetActive(true);
        }

        private void DisableTrail()
        {
            _trail.gameObject.SetActive(false);
        }
        
        private void CompleteExplosionJump()
        {
            _sequence = null;
            _agent.enabled = true;
            DisableTrail();
            _animator.SetBool(_isFallingHash, false);
            _owner.UnLock();
            _damageCallback.Invoke();
            _damageCallback = null;
        }

        private void OnDisable()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }

        private void PlayHitVfx()
        {
            var vfx = Instantiate(_groundhitEffect, _effectPosition.position, Quaternion.Euler(90, 0, 0));
            vfx.Play();
        }

        public void OnMessage(TimeStopStateChangedComponentMessage msg)
        {
            if(_sequence == null) return;

            if (msg.IsStopped)
                _sequence.Pause();
            else
                _sequence.Play();
        }
    }
}