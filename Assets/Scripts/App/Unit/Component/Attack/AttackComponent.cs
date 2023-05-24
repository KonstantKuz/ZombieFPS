
namespace App.Unit.Component.Attack
{
    public abstract class AttackComponent : IAttackComponent
    { 
        protected IAttackMediator Mediator { get; private set; }

        public void SetMediator(IAttackMediator mediator)
        {
            Mediator = mediator;
        }
    }
}