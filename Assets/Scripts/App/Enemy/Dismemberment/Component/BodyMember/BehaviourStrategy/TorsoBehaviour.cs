using System.Linq;
using Feofun.Extension;
using ModestTree;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.Profiling;

namespace App.Enemy.Dismemberment.Component.BodyMember.BehaviourStrategy
{
    public class TorsoBehaviour : MonoBehaviour, IBodyMemberBehaviour
    {
        [SerializeField] private Transform _torsoMeshesRoot;

        private RagdollDismembermentSystem.RagdollDismembermentSystem _ragdollDismembermentSystem;
        private Unit.Unit _unit;
        
        public bool ShouldDetach => !_ragdollDismembermentSystem.GetBonesForDismember().IsEmpty();
        private void Awake()
        {
            _ragdollDismembermentSystem = gameObject
                .RequireComponentInParent<RagdollDismembermentSystem.RagdollDismembermentSystem>();
        }
        
        public void OnEnable() => SetActiveTorso(true);
        public void Init(Unit.Unit data)
        {
            _unit = data;
            SetActiveTorso(true);
        }

        public void Kill()
        {
            if (ShouldDetach) {
                Dismember();
            }

            if (_unit.Health.IsAlive) {
                _unit.Health.Kill();
            }
            
            SetActiveTorso(false);
        }

        public void Dismember()
        {
            Profiler.BeginSample("[TorsoBehaviour]  Dismember all parts");
            DismemberAllParts();
            Profiler.EndSample();
        }

        private void SetActiveTorso(bool value)
        {
            gameObject.SetActive(value);
            _torsoMeshesRoot.gameObject.SetActive(value);
        }
        
        private void DismemberAllParts()
        {
            _ragdollDismembermentSystem
                .GetBonesForDismember()
                .ToList()
                .Select(it => it.GetComponent<BodyMemberBehaviour>())
                .ForEach(it => it.Dismember());
        }
    }
}