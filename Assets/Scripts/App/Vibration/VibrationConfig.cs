using System.Runtime.Serialization;
using Feofun.Config;

namespace App.Vibration
{
    [DataContract]
    public class VibrationConfig: ICollectionItem<VibrationType>
    {
        [DataMember(Name = "Id")] 
        private VibrationType _id;
        [DataMember]
        public float Time;
        public VibrationType Id => _id;

    }
}