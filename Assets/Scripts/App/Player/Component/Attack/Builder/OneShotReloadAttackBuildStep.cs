using App.Player.Component.Attack.Reloader;
using App.Unit.Component.Attack.Builder;
using UnityEngine;

namespace App.Player.Component.Attack.Builder
{ 
    public class OneShotReloadAttackBuildStep : PlayerAttackBuildStep
    {
        [SerializeField]
        private string _startReloadingAnimationName;
        [SerializeField]
        private string _mainReloadingAnimationName;

        [SerializeField]
        private bool _unloadClipBeforeReload;
        
        public override void Build(AttackBuilder attackBuilder, ReloadableInitData data)
        {
            var reloader = new OneShotWeaponReloader(data.WeaponState,
                _startReloadingAnimationName,
                _mainReloadingAnimationName,
                _unloadClipBeforeReload); 
            attackBuilder
                .Register<IWeaponReloader>(reloader)
                .Register(new AutomaticReloading(data.WeaponState, reloader));
        }
    }
}