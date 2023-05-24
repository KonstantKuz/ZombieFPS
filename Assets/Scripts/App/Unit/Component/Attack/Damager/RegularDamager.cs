using App.Unit.Component.Health;
using App.Unit.Model;
using Feofun.Components;
using Feofun.Extension;
using Logger.Extension;

namespace App.Unit.Component.Attack.Damager
{
    public class RegularDamager : AttackComponent, IDamager, IInitializable<AttackComponentInitData>
    {
        
        private IAttackModel _attackModel;
        public void Init(AttackComponentInitData data)
        {
            _attackModel = data.Unit.Model.AttackModel;
        }
        
        public virtual void Damage(HitInfo hitInfo)
        {
            var damageable = hitInfo.Target.RequireComponentInParent<IDamageable>();
            var damageInfo = DamageInfo.FromAttackAndHitInfo(_attackModel, hitInfo);
            damageable.TakeDamage(damageInfo);

            this.Logger().Trace($"Damage applied, target:= {hitInfo.Target.name}, damage = {damageInfo.Damage}");
        }
    }
}