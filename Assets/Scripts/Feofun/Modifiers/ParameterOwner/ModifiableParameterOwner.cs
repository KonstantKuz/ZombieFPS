using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Parameters;
using SuperMaxim.Core.Extensions;

namespace Feofun.Modifiers.ParameterOwner
{
    public class ModifiableParameterOwner : IModifiableParameterOwner
    {
        private readonly Dictionary<string, IModifiableParameter> _parameters = new();
        
        public void AddModifier(IModifier modifier)
        {
            modifier.Apply(this);
        }  
        public void AddModifiers(IEnumerable<IModifier> modifiers)
        {
            modifiers.ForEach(it=>it.Apply(this));
        }
        public IModifiableParameter GetParameter(string name)
        {
            if (!_parameters.ContainsKey(name))
            {
                throw new Exception($"No modifiable parameter named {name}");
            }
            return _parameters[name];
        }
        
        public void AddParameter(IModifiableParameter parameter)
        {
            if (_parameters.ContainsKey(parameter.Name))
            {
                throw new Exception($"UnitModel already have parameter named {parameter.Name}");
            }
            _parameters.Add(parameter.Name, parameter);
        }

        public void ResetAllParameters()
        {
            _parameters.Values.ForEach(it => it.Reset());
        }
        
        public void ReplaceModifiers(IEnumerable<IModifier> modifiers)
        {
            ResetAllParameters();
            modifiers.ForEach(it => it.Apply(this));
        }
    }
}