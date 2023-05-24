using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using SuperMaxim.Core.Extensions;

namespace App.Unit.Component.Attack.Condition
{
    public class AttackConditionComposite : AttackComponent, IAttackCondition, IDisposable
    {
        private readonly List<IAttackCondition> _conditions = new List<IAttackCondition>();
        public bool CanStartAttack => _conditions.IsEmpty() || _conditions.All(it => it.CanStartAttack);
        public bool CanFireImmediately => _conditions.IsEmpty() || _conditions.All(it => it.CanFireImmediately);

        public void AddConditions(List<IAttackCondition> condition) => _conditions.AddRange(condition);

        public void Dispose()
        {
            _conditions.OfType<IDisposable>().ForEach(it=>it.Dispose()); 
            _conditions.Clear();
        }
    }
}