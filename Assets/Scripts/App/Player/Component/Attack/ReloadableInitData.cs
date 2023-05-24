using App.Weapon.Component;

namespace App.Player.Component.Attack
{
    public readonly struct ReloadableInitData
    {
        public readonly Unit.Unit Unit;
        public readonly RuntimeInventoryWeaponState WeaponState;

        public ReloadableInitData(Unit.Unit unit, RuntimeInventoryWeaponState weaponState)
        {
            Unit = unit;
            WeaponState = weaponState;
        }
    }
}