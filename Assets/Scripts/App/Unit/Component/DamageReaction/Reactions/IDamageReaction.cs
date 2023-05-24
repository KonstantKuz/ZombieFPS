using App.Unit.Component.Health;

namespace App.Unit.Component.DamageReaction.Reactions
{
    public interface IDamageReaction
    { 
        void OnDamageReaction(DamageInfo damage);
    }
}