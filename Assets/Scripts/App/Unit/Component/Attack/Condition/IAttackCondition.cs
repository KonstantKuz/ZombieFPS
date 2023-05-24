namespace App.Unit.Component.Attack.Condition
{
    public interface IAttackCondition : IAttackComponent
    {
        bool CanStartAttack { get; }
        bool CanFireImmediately { get; }
    }
}