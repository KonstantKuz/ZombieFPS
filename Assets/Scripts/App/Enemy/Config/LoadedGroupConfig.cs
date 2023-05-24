using System.Runtime.Serialization;

namespace App.Enemy.Config
{
    [DataContract]
    public class LoadedGroupConfig
    {
        [DataMember]
        public int RoundNumber;
        [DataMember] 
        public float StartDelay;
        [DataMember]
        public int WaveNumber;
        [DataMember]
        public int EnemiesCountBeforeSpawn;
        [DataMember]
        public string EnemyId;
        [DataMember]
        public string SpawnPointId;
        [DataMember]
        public float SpawnTime;
        [DataMember]
        public int Count;
    }
}