using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit.Component.DamageReaction.Reactions;
using App.Unit.Component.Health;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.Unit.Component.DamageReaction
{
    [RequireComponent(typeof(IDamageable))]
    public class ReactionDamageObserver : MonoBehaviour
    {
        private IDamageable _damageable;

        private IEnumerable<IDamageReaction> _reactions;

        private void Awake()
        {
            _damageable = gameObject.GetComponent<IDamageable>();
            _reactions = gameObject.GetComponents<IDamageReaction>();
        }

        private void OnEnable()
        {
            _damageable.OnDamageTaken += OnDamageTakenReaction;
        }

        private void OnDamageTakenReaction(DamageInfo damage)
        {
            if (gameObject == null) {
                return;
            }
            _reactions.ForEach(it => it.OnDamageReaction(damage));
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _reactions.OfType<IDisposable>().ForEach(it => it.Dispose());
            _damageable.OnDamageTaken -= OnDamageTakenReaction;
        }
    }
}