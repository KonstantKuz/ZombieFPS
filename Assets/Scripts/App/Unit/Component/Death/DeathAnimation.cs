using System.Collections;
using App.Unit.Extension;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;

namespace App.Unit.Component.Death
{
    public class DeathAnimation : MonoBehaviour, IUnitDeath, IInitializable<Unit>
    { 
        private readonly int _deathHash = Animator.StringToHash("Death");
       
        [SerializeField]
        private float _delayUntilDisappear;
        
        private Animator _animator;      
        private DeathTween _deathTween;
        private Unit _owner;

        private void Awake()
        {
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _deathTween = gameObject.RequireComponent<DeathTween>();

        }
        
        public void Init(Unit owner) => _owner = owner;

        public void PlayDeath()
        {
            StartCoroutine(Disappear());
        }
        
        private IEnumerator Disappear()
        {
            while (_animator.IsInFallingState()) yield return null;
            
            _animator.SetTrigger(_deathHash);            
            yield return new WaitForSeconds(_delayUntilDisappear);
            yield return _deathTween.Play(); 
            _owner.Destroy();
        }


    }
}
