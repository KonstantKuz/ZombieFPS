using System;
using App.Unit.Component.ComponentProvider;

namespace App.Unit.Component.Attack
{
    public interface IAttackMediator : IComponentProvider<IAttackComponent>, IComponentProviderUser<IAttackComponent>, IDisposable
    {
        bool CanAttack { get; }
        bool IsAttacking { get; }
        event Action OnFire;
        event Action OnDamaged;
        void Attack();
        void Damage(HitInfo hitInfo);
    }
}