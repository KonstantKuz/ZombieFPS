using System.Runtime.Serialization;
using App.Items.Data;
using Feofun.Config;

namespace App.Items.Config
{
    [DataContract]
    public class ItemConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public ItemType Type;
        public string Id => _id;
    }
}