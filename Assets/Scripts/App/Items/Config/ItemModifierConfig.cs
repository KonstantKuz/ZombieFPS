using System.Runtime.Serialization;
using Feofun.Modifiers.Config;

namespace App.Items.Config
{
    public enum ItemModifierTarget
    {
        Weapon,
        Unit
    }
    [DataContract]
    public class ItemModifierConfig
    {
        [DataMember]
        public ModifierConfig ModifierConfig;
        [DataMember(Name = "Target")]
        public ItemModifierTarget Target;
    }
}