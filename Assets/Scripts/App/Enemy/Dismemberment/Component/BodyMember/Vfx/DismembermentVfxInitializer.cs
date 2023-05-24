using System.Collections.Generic;
using System.Linq;
using App.Enemy.Dismemberment.Config;
using App.Unit.Component.Vfx;
using Feofun.Extension;
using UnityEngine;
using Zenject;

namespace App.Enemy.Dismemberment.Component.BodyMember.Vfx
{
    public class DismembermentVfxInitializer : MonoBehaviour
    {
        [SerializeField] private DismembermentVfxConfig _vfxConfig;
        [Inject] private DiContainer _container;

        private List<BodyMemberVfxInitializer> _memberVfxInitializers;
        private void Awake()
        {
            _memberVfxInitializers = GetComponentsInChildren<BodyMemberBehaviour>(true)
                .Select(CreateVfxComponents).ToList();
            var unit = gameObject.GetComponentInParent<Unit.Unit>();
            var unitVfxPlayer = unit == null ? null : unit.GetComponent<IVfxPlayer>();
            _memberVfxInitializers.ForEach(it => it.InitVfxPlayer(unitVfxPlayer, _vfxConfig));
        }

        private BodyMemberVfxInitializer CreateVfxComponents(BodyMemberBehaviour bodyMember)
        {
            return new BodyMemberVfxInitializer(_container, bodyMember);
        }

        private void OnDestroy()
        {
            _memberVfxInitializers.ForEach(it => it.Dispose());
        }
    }
}