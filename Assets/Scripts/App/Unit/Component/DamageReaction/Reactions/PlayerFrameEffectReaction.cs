using App.UI.Components.FrameEffects;
using App.UI.Components.FrameEffects.Data;
using App.Unit.Component.Health;
using UnityEngine;
using Zenject;

namespace App.Unit.Component.DamageReaction.Reactions
{
    public class PlayerFrameEffectReaction : MonoBehaviour, IDamageReaction
    {
        private const string POISON_ZOMBIE_ATTACK_NAME = "PoisonZombieAttack";
        [Inject]
        private FrameEffectsPlayer _frameEffectsPlayer;
        
        public void OnDamageReaction(DamageInfo damage)
        {
            if (!damage.AttackName.Equals(POISON_ZOMBIE_ATTACK_NAME)) {
                _frameEffectsPlayer.Play(FrameEffectType.Damage);
                return;
            }
            if (!_frameEffectsPlayer.IsPlaying(FrameEffectType.AcidDamage)) {
                _frameEffectsPlayer.Play(FrameEffectType.AcidDamage);
            }
        }
    }
}
