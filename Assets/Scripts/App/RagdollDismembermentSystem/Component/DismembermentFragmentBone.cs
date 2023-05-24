using System;
using EasyButtons;
using Feofun.Extension;
using UnityEngine;

namespace App.RagdollDismembermentSystem.Component
{
    public class DismembermentFragmentBone : MonoBehaviour
    {
        private RagdollDismembermentSystem _dismembermentSystem;
        public bool Detached => _dismembermentSystem.IsDismembered(this);

        public void Awake()
        {
            _dismembermentSystem = gameObject.RequireComponentInParent<RagdollDismembermentSystem>();
        }
        
        [Button]
        public void Dismember()
        {
            if (Detached){
                throw new Exception($"Fragment already detached, name:= {transform.name}");
            }
            _dismembermentSystem.Dismember(this);
        }


        public void OnDestroy()
        {
            if (_dismembermentSystem != null) {
                _dismembermentSystem.OnDestroyCrackedBone(this);
            }
        }
    }
}