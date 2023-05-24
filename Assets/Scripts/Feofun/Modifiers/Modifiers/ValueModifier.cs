using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace Feofun.Modifiers.Modifiers
{
    public class ValueModifier: IModifier
    {
        private readonly string _paramName;
        private readonly float _value;
        
        public string ParamName => _paramName;
        public float ModifierValue => _value;

        public ValueModifier(string paramName, float value)
        {
            _paramName = paramName;
            _value = value;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            parameter.AddValue(_value);
        }
    }
}