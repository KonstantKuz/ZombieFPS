using System.Runtime.Serialization;

namespace App.Enemy.Config
{
    [DataContract]
    public class EnemyAttackConfig
    {
        [DataMember]
        public float AttackInterval;
        [DataMember] 
        public float AttackDistance;
        [DataMember] 
        public float AttackDamage;
        [DataMember] 
        public string AttackName;
    }
}