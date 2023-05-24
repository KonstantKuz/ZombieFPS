using System.Runtime.Serialization;

namespace App.InteractableItems.Config
{
    [DataContract]
    public class InteractableItemConfig
    {
        [DataMember(Name = "Id")]
        public string Id;
        [DataMember]
        public string Icon;
    }
}