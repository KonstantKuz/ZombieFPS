using App.Enemy.Component.Destroy;
using App.RagdollDismembermentSystem.Component;
using App.Unit.Component.Death;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.BehaviourStrategy
{
    [RequireComponent(typeof(AutoDestroyByTimeoutWhenNotVisible))]
    [RequireComponent(typeof(Destroyer))]
    public class LimbsBehaviour : MonoBehaviour, IBodyMemberBehaviour
    {
        private DismembermentFragmentBone _dismembermentBone;
        private AutoDestroyByTimeoutWhenNotVisible _destroyByTimeout;
        private Destroyer _destroyer;
 
        public bool ShouldDetach => !_dismembermentBone.Detached;
        private void Awake()
        {
            _dismembermentBone = gameObject.RequireComponent<DismembermentFragmentBone>();
            _destroyByTimeout = GetComponent<AutoDestroyByTimeoutWhenNotVisible>();
            _destroyer = gameObject.RequireComponent<Destroyer>();
        }

        public void Init(Unit.Unit data) { }

        public void Kill()
        {
            if (ShouldDetach) {
                Dismember();
            }
            _destroyer.Destroy();
        }

        public void Dismember()
        {
            _dismembermentBone.Dismember();
            _destroyByTimeout.StartTimeout();
        }
    }
}