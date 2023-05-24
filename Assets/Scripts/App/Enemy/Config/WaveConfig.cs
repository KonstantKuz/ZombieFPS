using System.Collections.Generic;

namespace App.Enemy.Config
{
    public class WaveConfig
    {
        public int WaveNumber;
        public int EnemiesCountBeforeStart;
        public List<GroupConfig> Groups;
    }
}