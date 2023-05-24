using System.Collections.Generic;
using System.Linq;

namespace App.Enemy.Dismemberment.Config
{
    public class EnemyBodyConfig
    {
        public IReadOnlyList<BodyMemberConfig> Members { get; }

        public EnemyBodyConfig(IEnumerable<BodyMemberConfig> bodyMemberConfigs)
        {
            Members = bodyMemberConfigs.ToList();
        }
    }
}