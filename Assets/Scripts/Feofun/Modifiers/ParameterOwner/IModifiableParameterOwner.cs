using Feofun.Modifiers.Parameters;
using JetBrains.Annotations;

namespace Feofun.Modifiers.ParameterOwner
{
    public interface IModifiableParameterOwner
    {
        [NotNull] IModifiableParameter GetParameter(string name);
        void AddParameter(IModifiableParameter parameter);
    }
}