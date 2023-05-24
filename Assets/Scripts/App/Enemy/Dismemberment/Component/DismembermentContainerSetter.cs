using App.Unit.Extension;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace App.Enemy.Dismemberment.Component
{
    [RequireComponent(typeof(RagdollDismembermentSystem.RagdollDismembermentSystem))]
    public class DismembermentContainerSetter : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [Inject] 
        private Feofun.World.World _world;

        private RagdollDismembermentSystem.RagdollDismembermentSystem _ragdollDismembermentSystem;
        private RagdollDismembermentSystem.RagdollDismembermentSystem RagdollDismembermentSystem =>
            _ragdollDismembermentSystem ??= gameObject.GetComponent<RagdollDismembermentSystem.RagdollDismembermentSystem>();


        public void Init(Unit.Unit unit)
        {
            var detachingContainer = unit.HasPool() ? _world.PoolContainer : _world.SpawnContainer;
            RagdollDismembermentSystem.DetachingContainer = detachingContainer;
        }
    }
}