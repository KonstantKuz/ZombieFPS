using App.Unit.Component.Target;
using App.Weapon;
using App.Weapon.Weapons;
using Feofun.Components;
using Feofun.Extension;

namespace App.Unit.Component.Attack.Condition
{
    public class RegularAttackCondition : AttackComponent, IInitializable<AttackComponentInitData>, IAttackCondition
    {
        private Unit _owner;
        private BaseWeapon _weapon;

        public bool CanStartAttack => _owner.IsActive &&
                                      _weapon.IsWeaponReady &&
                                      _owner.TargetProvider.DistanceToTarget(_owner.SelfTarget.Root.position) <=
                                      _owner.Model.AttackModel.AttackDistance;

        public bool CanFireImmediately => _owner.TargetProvider.Target.IsValid();
        
        
        public void Init(AttackComponentInitData data)
        {
            _owner = data.Unit;
            _weapon = data.AttackRoot.gameObject.RequireComponentInChildren<BaseWeapon>();
        }
    }
}