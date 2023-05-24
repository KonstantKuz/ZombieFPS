using System.Runtime.Serialization;

namespace App.Level.Config
{
    [DataContract]
    public class LevelConfig
    {
        [DataMember(Name = "LevelId")]
        public string LevelId;
    }
}