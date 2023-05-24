using System.Runtime.Serialization;
using Feofun.Config;

namespace App.Enemy.Config
{
    [DataContract]
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public float Health;
        [DataMember] 
        public float MoveSpeed;
        [DataMember] 
        public float CrawlSpeed;
        [DataMember] 
        public EnemyAttackConfig AttackConfig;
        
        public string Id => _id;
    }
}