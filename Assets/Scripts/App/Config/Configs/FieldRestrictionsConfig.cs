using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Modifiers.Data;

namespace App.Config.Configs
{
    [DataContract]
    public class FieldRestrictionsConfig: ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember]
        public float MinValue;
        [DataMember] 
        public float MaxValue;
        
        public string Id => _id;

        public FieldRange GetFieldRange() => new FieldRange(MinValue, MaxValue);
    }
}