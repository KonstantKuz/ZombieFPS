using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;
using JetBrains.Annotations;

namespace App.Enemy.Dismemberment.Config
{
    public class EnemyBodyMembersConfig : ILoadableConfig
    {
        private IReadOnlyDictionary<string, EnemyBodyConfig> _enemyBodyConfigs;

        public void Load(Stream stream)
        {
            _enemyBodyConfigs = new CsvSerializer().ReadNestedTable<BodyMemberConfig>(stream)
                .ToDictionary(it => it.Key, it => new EnemyBodyConfig(it.Value));
        }
        public EnemyBodyConfig GetBodyConfig(string enemyId)
        {
            if (!_enemyBodyConfigs.ContainsKey(enemyId)) {
                throw new NullReferenceException($"No EnemyBodyConfig for id {enemyId} in EnemyBodyMembersConfig");
            }
            return _enemyBodyConfigs[enemyId];
        }

        [CanBeNull]
        public EnemyBodyConfig Find(string enemyId)
        {
            return _enemyBodyConfigs.ContainsKey(enemyId) ? GetBodyConfig(enemyId) : null;
        }
    }
}