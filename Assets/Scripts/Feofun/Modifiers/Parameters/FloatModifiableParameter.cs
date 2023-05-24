using Feofun.Modifiers.Data;
using Feofun.Modifiers.ParameterOwner;
using UniRx;
using UnityEngine;

namespace Feofun.Modifiers.Parameters
{
    public class FloatModifiableParameter : IModifiableParameter
    {
        private readonly FieldRange _fieldRange;
        private readonly ReactiveProperty<float> _reactiveValue;
        
        private float _value;
        public float InitialValue { get; }
        public string Name { get; }
        public IReadOnlyReactiveProperty<float> ReactiveValue => _reactiveValue;

        public FloatModifiableParameter(string name, float initialValue, IModifiableParameterOwner owner, FieldRange fieldRange = new())
        {
            Name = name;
            _fieldRange = fieldRange;
            _reactiveValue = new ReactiveProperty<float>();
            Value = InitialValue = initialValue;
            owner.AddParameter(this);
        }

        public void AddValue(float amount)
        {
            Value += amount;
        }
        
        public void OverrideValue(float value)
        {
            Value = value;
        }
        
        public void Reset()
        {
            Value = InitialValue;
        }
        public float Value
        {
            get => _value;
            //For parameters with max value = 100% we allow to overflow, cause in case of 50 + 50 + 30 - 20 we should have 100, not 80 as result
            //value of more than 100 should be checked at place of application and applied correctly
            private set
            {
                _value = Mathf.Max(Mathf.Min(value, _fieldRange.Max), _fieldRange.Min);
                _reactiveValue.SetValueAndForceNotify(_value);
            }
        }


    }
}