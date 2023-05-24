using App.Ragdoll;
using Feofun.Extension;
using UnityEngine;

namespace App.Unit.Component.Death
{
    public class RagdollDeath : MonoBehaviour, IUnitDeath
    {
        private IRagdoll _ragdoll;
        
        private void Awake()
        {
            _ragdoll = gameObject.RequireComponent<IRagdoll>();
        }
        public void PlayDeath()
        {
            _ragdoll.Enable();
        }

        private void OnDisable()
        {
            _ragdoll.Disable();
        }
    }
}
