namespace App.Unit.Model
{
    public interface IUnitModel
    {
        float MoveSpeed { get; }
        IHealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
    }
}