using System.Collections.Generic;
using System.Runtime.Serialization;

namespace App.Enemy.Config
{
    [DataContract]
    public class LoadedWaveConfig
    {
        [DataMember]
        public string LevelId;
        public List<GroupConfig> Groups;
    }
}