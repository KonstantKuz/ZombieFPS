using App.Unit.Component.Health;

namespace App.Enemy.Dismemberment.Component.BodyMember
{
    public class BodyMemberHealth: Health
    {
        public override void TakeDamage(DamageInfo damage)
        {
            var parentDamageable = transform.GetComponentInParent<UnitHealth>();
            parentDamageable?.TakeDamage(damage);
            base.TakeDamage(damage); 

        }
    }
}