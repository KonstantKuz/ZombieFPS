using System.Runtime.Serialization;
using Feofun.Config;

namespace App.Items.Config
{
    [DataContract]
    public class StartingItemConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public bool Equipped;    
        [DataMember] 
        public int SlotIndex;
        public string Id => _id;
    }
}