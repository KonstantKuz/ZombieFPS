using Feofun.Components;
using Logger.Extension;
using UnityEngine;

namespace App.Unit.Component.Attack
{
    public abstract class BaseAttack : MonoBehaviour, IInitializable<Unit>
    {
        protected IAttackMediator _attack;
        public bool Enabled { get; set; } = true;
        public bool CanAttack => Enabled && _attack != null && _attack.CanAttack;
        public bool IsAttacking => _attack.IsAttacking;

        public virtual void Init(Unit owner)
        {
            Dispose();
            _attack = BuildAttack(owner);
        }

        protected abstract IAttackMediator BuildAttack(Unit owner);

        public abstract void DisposeLastAttack();

        public void Attack()
        {
            if (!CanAttack)
            {
                this.Logger().Error("Can not start attack");
                return;
            }

            _attack.Attack();
        }

        private void OnDisable() => Dispose();

        protected virtual void Dispose()
        {
            _attack?.Dispose();
            _attack = null;
        }
    }
}