using App.Weapon.Component;
using App.Weapon.Service;
using Feofun.Util.Timer;
using UniRx;

namespace App.UI.Screen.World.Player.RuntimeInventory.Model
{
    public class WeaponViewModel
    {
        public readonly IReactiveProperty<ITimer> ReloadingTimer = new ReactiveProperty<ITimer>(null);
        public RuntimeInventoryWeaponState RuntimeInventoryState { get; }

        public WeaponViewModel(WeaponService weaponService, string itemId)
        {
            RuntimeInventoryState = weaponService.GetRuntimeWeaponState(itemId);
            if(!weaponService.IsActiveWeapon(itemId)) return;
            ReloadingTimer = weaponService.RequirePlayerAttack().ReloadingTimer;
        }
    }
}