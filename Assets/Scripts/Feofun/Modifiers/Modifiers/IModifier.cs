using Feofun.Modifiers.ParameterOwner;

namespace Feofun.Modifiers.Modifiers
{
    public interface IModifier
    {
        string ParamName { get; }
        float ModifierValue { get; }
        public void Apply(IModifiableParameterOwner parameterOwner);
    }
}