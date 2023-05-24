using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit;
using Feofun.Components;

namespace App.Player.Component.Attack
{
    public partial class PlayerAttackHolder
    {
        private class CurrentAttackData
        {
            private WeaponRemovalAnimationHandler _removeAnimationHandler;
            private readonly ISet<IUpdatableComponent> _innerUpdatables;
            private readonly IUpdatablesList _updatablesList;
            private readonly Action<PlayerReloadableAttack> _attackDestructor;
            
            public PlayerReloadableAttack Attack { get; }

            public CurrentAttackData(PlayerReloadableAttack attack,
                IUpdatablesList updatablesList,
                Action<PlayerReloadableAttack> attackDestructor)
            {
                Attack = attack;
                _updatablesList = updatablesList;
                _attackDestructor = attackDestructor;
                _innerUpdatables = attack.GetComponentsInChildren<IUpdatableComponent>().ToHashSet();
                _updatablesList.AddUpdatables(_innerUpdatables);
            }

            public void Remove(Action onComplete, bool playHideAnimation)
            {
                CancelRemoval();
                Action removeAttack = Dispose;
                removeAttack += onComplete;
                if (playHideAnimation)
                {
                    _removeAnimationHandler = new WeaponRemovalAnimationHandler(removeAttack, Attack.transform);
                }
                else
                {
                    removeAttack.Invoke();
                }
            }

            public void Dispose()
            {
                CancelRemoval();
                _updatablesList.RemoveUpdatables(_innerUpdatables);
                _attackDestructor?.Invoke(Attack);
            }

            public void CancelRemoval()
            {
                _removeAnimationHandler?.Dispose();
                _removeAnimationHandler = null;
            }
        }
    }
}