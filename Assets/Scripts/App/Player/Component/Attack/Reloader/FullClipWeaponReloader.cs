using App.Extension;
using App.Weapon.Component;

namespace App.Player.Component.Attack.Reloader
{
    public class FullClipWeaponReloader : WeaponReloaderBase
    {
        
        public FullClipWeaponReloader(RuntimeInventoryWeaponState weaponState) : base(weaponState)
        {
        }

        public override void StartReload()
        {
            DisposeTimer();
            StartTimer(_weaponState.Model.ReloadTime);
        }

        protected override void OnReloaded()
        { 
            Clip.Load(Clip.GetAmmoCountForFullLoad());
            DisposeTimer();
        }
    }
}