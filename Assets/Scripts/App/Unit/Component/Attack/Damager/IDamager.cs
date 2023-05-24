namespace App.Unit.Component.Attack.Damager
{
    public interface IDamager : IAttackComponent
    {
        void Damage(HitInfo hitInfo);
    }
}