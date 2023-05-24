using App.Player.Model.Attack;
using App.Unit.Extension;
using Feofun.Components;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class PlayerAccuracyModifier : AccuracyModifier, IInitializable<Unit.Unit>
    {
        private Unit.Unit _owner;
        public void Init(Unit.Unit data)
        {
            _owner = data;
        }

        protected override float GetRandomSpreadAngle()
        {
            return GetCurrentAccuracyPercent() * base.GetRandomSpreadAngle();
        }

        private float GetCurrentAccuracyPercent()
        {
            var attackModel = _owner.RequireAttackModel<ReloadableWeaponModel>();
            return attackModel.GetAccuracyInvertedPercent();
        }
    }
}