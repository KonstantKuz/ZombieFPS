using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace Feofun.Modifiers.Modifiers
{
    public class PercentModifier: IModifier
    {
        private readonly string _paramName;
        private readonly float _percentValue;
        private readonly bool _isIncreasing;

        public string ParamName => _paramName;
        public float ModifierValue => _percentValue;
        public bool IsIncreasing => _isIncreasing;

        private int Direction => _isIncreasing ? +1 : -1;
        public PercentModifier(string paramName, float percentValue, bool isIncreasing)
        {
            _paramName = paramName;
            _percentValue = percentValue;
            _isIncreasing = isIncreasing;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            var valueDelta = parameter.InitialValue * _percentValue / 100;
            parameter.AddValue(Direction * valueDelta);

        }
    }
}