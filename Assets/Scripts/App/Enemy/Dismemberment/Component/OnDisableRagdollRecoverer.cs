using Feofun.Core;
using Feofun.Extension;
using Feofun.World.Extesion;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component
{ 
    public class OnDisableRagdollRecoverer : MonoBehaviour
    {
        private Feofun.World.World _world;
        private RagdollDismembermentSystem.RagdollDismembermentSystem _ragdollDismembermentSystem;

        private void Awake()
        {
            _world = AppContext.Container.Resolve<Feofun.World.World>();
            _ragdollDismembermentSystem = gameObject
                .RequireComponentInChildren<RagdollDismembermentSystem.RagdollDismembermentSystem>();
        }

        public void OnDisable()
        {
            if (_world.IsPoolActivated()) {
                _ragdollDismembermentSystem.RecoverAllFragments();
            }
        }
    }
}