using App.Config.Configs;
using Feofun.Config;
using Feofun.Core;
using Feofun.Modifiers.Data;
using Feofun.Modifiers.ParameterOwner;
using Feofun.Modifiers.Parameters;

namespace App.Modifiers
{
    public class FloatModifiableCreateHelper
    {
        private readonly StringKeyedConfigCollection<FieldRestrictionsConfig> _fieldRestrictions;

        public FloatModifiableCreateHelper()
        {
            _fieldRestrictions = AppContext
                .Container
                .Resolve<StringKeyedConfigCollection<FieldRestrictionsConfig>>();
        }

        public FloatModifiableParameter CreateWithRange(string name,
            float initialValue,
            IModifiableParameterOwner owner) =>
            new(name, initialValue, owner, GetFieldRange(name));
        
        private FieldRange GetFieldRange(string fieldName) => _fieldRestrictions.Get(fieldName).GetFieldRange();
    }
}