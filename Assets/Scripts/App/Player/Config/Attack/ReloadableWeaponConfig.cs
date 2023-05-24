using System.Runtime.Serialization;
using App.Vibration;
using Feofun.Config;

namespace App.Player.Config.Attack
{
    [DataContract]
    public class ReloadableWeaponConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember]
        public float Damage;
        [DataMember] 
        public float FireRate;
        [DataMember]
        public int ClipSize;
        [DataMember] 
        public float ReloadTime;   
        [DataMember] 
        public float Accuracy;    
        [DataMember] 
        public float Control;    
        [DataMember] 
        public float Distance;     
        [DataMember] 
        public float ProjectileSpeed;    
        [DataMember] 
        public float DamageRadius;
        [DataMember] 
        public VibrationType Vibration;
        [DataMember] 
        public int ShotCount;

        public string Id => _id;
    }
}