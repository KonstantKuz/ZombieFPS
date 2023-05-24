using System.Linq;
using App.Enemy.Dismemberment.Component;
using App.Enemy.Dismemberment.Component.BodyMember;
using App.Enemy.Dismemberment.Model;
using App.Unit.Component.Death;
using App.Unit.Extension;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Enemy.Component.Destroy
{
    public class UnitDestroyerInitializer : MonoBehaviour, IInitializable<Unit.Unit>
    {
        private Destroyer _unitDestroyer;
        private Destroyer[] _membersDestroyers;
        [CanBeNull]
        private RagdollDismembermentSystem.RagdollDismembermentSystem _ragdollDismembermentSystem;
        private void Awake()
        {
            _unitDestroyer = gameObject.RequireComponent<Destroyer>();
            _membersDestroyers = gameObject.GetComponentsInChildren<BodyMemberBehaviour>()
                .Where(it=>it.BodyMemberType != BodyMemberType.Torso)
                .Select(it => it.gameObject.GetOrAddComponent<Destroyer>())
                .ToArray();
            _ragdollDismembermentSystem = GetComponentInChildren<RagdollDismembermentSystem.RagdollDismembermentSystem>();

        }
        public void Init(Unit.Unit unit)
        {
            var hasPool = unit.HasPool();
            InitUnitDestroyer(hasPool);
            InitMembersDestroyers(hasPool);
            AddOnDisableRagdollRecoverer(hasPool);
        }
        private void AddOnDisableRagdollRecoverer(bool hasPool)
        {
            if (hasPool && _ragdollDismembermentSystem != null) {
                gameObject.GetOrAddComponent<OnDisableRagdollRecoverer>();
            }
        }

        private void InitUnitDestroyer(bool hasPool) => _unitDestroyer.DestroyType = hasPool ? DestroyType.Pool : DestroyType.Destroy;

        private void InitMembersDestroyers(bool hasPool)
        {
            foreach (var membersDestroyer in _membersDestroyers)
            {
                membersDestroyer.DestroyType = hasPool ? DestroyType.Inactive : DestroyType.Destroy;
                if (!hasPool) {
                    membersDestroyer.gameObject.GetOrAddComponent<AutoDestroyableByWorldCleanUp>();
                }
            }
        }
        
    }
}