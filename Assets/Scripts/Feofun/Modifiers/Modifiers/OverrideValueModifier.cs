using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace Feofun.Modifiers.Modifiers
{
    public class OverrideValueModifier : IModifier
    {
        private readonly string _paramName;
        private readonly float _value;

        public string ParamName => _paramName;
        public float ModifierValue => _value;

        public OverrideValueModifier(string paramName, float value)
        {
            _paramName = paramName;
            _value = value;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            parameter.OverrideValue(_value);
        }
    }
}