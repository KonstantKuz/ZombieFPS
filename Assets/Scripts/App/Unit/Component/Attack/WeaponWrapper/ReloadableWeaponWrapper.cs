using App.Player.Model.Attack;
using App.Unit.Component.Attack.Condition;
using App.Weapon.Component;
using App.Weapon.Weapons;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine.Profiling;

namespace App.Unit.Component.Attack.WeaponWrapper
{
    public class ReloadableWeaponWrapper : AttackComponent, IInitializable<AttackComponentInitData>, IWeaponWrapper, IAttackCondition
    {
        private Unit _owner;
        private BaseWeapon _weapon;
        private readonly RuntimeInventoryWeaponState _runtimeInventoryState;

        public bool CanStartAttack => _runtimeInventoryState.HasAmmo;
        public bool CanFireImmediately => _runtimeInventoryState.HasAmmo;

        public ReloadableWeaponWrapper(RuntimeInventoryWeaponState runtimeInventoryState)
        {
            _runtimeInventoryState = runtimeInventoryState;
        }
        public void Init(AttackComponentInitData data)
        {
            _owner = data.Unit;
            _weapon = data.AttackRoot.gameObject.RequireComponentInChildren<BaseWeapon>();
            if (_weapon is MultiShootWeapon weapon)
            {
                weapon.Init(((ReloadableWeaponModel) _owner.Model.AttackModel).ShotCount);
            }
        }

        public void Fire()
        {
            Profiler.BeginSample("ReloadableWeaponWrapper.Fire");
            _weapon.Fire(_owner.TargetProvider.Target, hitInfo => Mediator.Damage(hitInfo), 
                _runtimeInventoryState.GetProjectileParams());
            _runtimeInventoryState.OnFire();
            Profiler.EndSample();
        }
    }
}