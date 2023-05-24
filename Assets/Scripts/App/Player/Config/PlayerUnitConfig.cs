using System.Runtime.Serialization;
using Feofun.Config;

namespace App.Player.Config
{
    [DataContract]
    public class PlayerUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public float Health;
        [DataMember] 
        public float MoveSpeed;
        [DataMember] 
        public float RunningSpeedFactor;
        [DataMember] 
        public float Regeneration;
        [DataMember] 
        public int UnlockLevel;

        public string Id => _id;

        public bool IsLocked(int playerLevel) => UnlockLevel > playerLevel;
    }
}