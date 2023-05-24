using App.Player.Component.Attack.Reloader;
using App.Unit.Component.Attack.Builder;
using UnityEngine;

namespace App.Player.Component.Attack.Builder
{
    public class FullClipReloadAttackBuildStep : PlayerAttackBuildStep
    {
        [SerializeField] 
        private string _reloadingAnimation;
        
        public override void Build(AttackBuilder attackBuilder, ReloadableInitData data)
        {
            var reloader = new FullClipWeaponReloader(data.WeaponState); 
            attackBuilder
                .Register<IWeaponReloader>(reloader)
                .Register(new FullClipReloadingAnimation(data.WeaponState, reloader, _reloadingAnimation))
                .Register(new AutomaticReloading(data.WeaponState, reloader));
        }
    }
}