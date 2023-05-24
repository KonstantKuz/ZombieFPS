using System.Runtime.Serialization;
using App.Enemy.Dismemberment.Model;

namespace App.Enemy.Dismemberment.Config
{
    public class BodyMemberConfig
    {
        [DataMember(Name = "BodyMemberType")]
        public BodyMemberType Type;
        [DataMember]
        public float MainHealth;
        [DataMember]
        public float ExtraHealth;

    }
}