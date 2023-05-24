using App.Weapon.Weapons;
using Feofun.Components;
using Feofun.Extension;

namespace App.Unit.Component.Attack.WeaponWrapper
{
    public class WeaponWrapper: AttackComponent, IInitializable<AttackComponentInitData>, IWeaponWrapper
    {
        private Unit _owner;
        private BaseWeapon _weapon;
        
        public void Init(AttackComponentInitData data)
        {
            _owner = data.Unit;
            _weapon = data.AttackRoot.gameObject.RequireComponentInChildren<BaseWeapon>();
        }
        public void Fire()
        {
            _weapon.Fire(_owner.TargetProvider.Target, 
                target => Mediator.Damage(target), null);
        }
        
    }
}