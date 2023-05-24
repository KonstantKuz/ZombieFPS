using App.Modifiers;
using App.Unit.Model;
using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace App.Player.Model
{
    public class PlayerHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;
        
        public float MaxHealth => _maxHealth.Value;
        public float Regeneration { get; }

        
        public PlayerHealthModel(float maxHealth, float regeneration, IModifiableParameterOwner parameterOwner)
        {
            var parameterCreator = new FloatModifiableCreateHelper();
            _maxHealth = parameterCreator.CreateWithRange(ParameterNames.HEALTH, maxHealth, parameterOwner);
            Regeneration = regeneration;
        }
    }
}