using System.Runtime.Serialization;

namespace App.Settings
{
    [DataContract]
    public class SettingsData
    {
        [DataMember]
        public bool SoundEnabled = true;
        [DataMember]
        public bool VibrationEnabled = true;
    }
}