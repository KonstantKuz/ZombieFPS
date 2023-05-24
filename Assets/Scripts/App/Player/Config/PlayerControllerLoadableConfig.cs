using System.Runtime.Serialization;

namespace App.Player.Config
{
    [DataContract]
    public class PlayerControllerLoadableConfig
    {
        [DataMember]
        public float SensitivityCoefficient;
    }
}