using App.Unit.Component.Attack;
using App.Unit.Model;

namespace App.Unit.Component.Health
{
    public readonly struct DamageInfo
    {
        public readonly float Damage;
        public readonly string AttackName;

        public DamageInfo(float damage, string attackName)
        {
            Damage = damage;
            AttackName = attackName;
        }

        public static DamageInfo FromAttackAndHitInfo(IAttackModel attackModel, HitInfo hitInfo) =>
            new(attackModel.HitDamage * hitInfo.HitFraction, attackModel.Name);
    }
}