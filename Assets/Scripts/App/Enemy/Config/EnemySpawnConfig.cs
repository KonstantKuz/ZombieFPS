using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace App.Enemy.Config
{
    public class EnemySpawnConfig : ILoadableConfig
    {
        public IReadOnlyDictionary<string, IReadOnlyList<LoadedGroupConfig>> LevelConfigs { get; private set; }

        public void Load(Stream stream)
        {
            LevelConfigs = new CsvSerializer().ReadNestedTable<LoadedGroupConfig>(stream)
                .ToDictionary(it => it.Key, it => it.Value);
        }

        public IEnumerable<RoundConfig> GetRoundsForLevel(string levelId)
        {
            if (!LevelConfigs.ContainsKey(levelId))
            {
                throw new ArgumentException($"[EnemySpawnConfig] Rounds config with waves not found for level := {levelId}");
            }

            return LevelConfigs[levelId]
                .OrderBy(it => it.RoundNumber)
                .GroupBy(it => it.RoundNumber)
                .Select(round => new RoundConfig
                {
                    RoundNumber = round.Key,
                    StartDelay = round.First().StartDelay,
                    Waves = GetWavesForRound(round).ToList(),
                });
        }

        private IEnumerable<WaveConfig> GetWavesForRound(IEnumerable<LoadedGroupConfig> roundWaves)
        {
            return roundWaves.OrderBy(it => it.WaveNumber)
                .GroupBy(it => it.WaveNumber)
                .Select(it => BuildWaveConfig(it.Key, it.ToList()));
        }

        private WaveConfig BuildWaveConfig(int waveNumber, List<LoadedGroupConfig> loadedGroups)
        {
            return new WaveConfig
            {
                WaveNumber = waveNumber,
                EnemiesCountBeforeStart = loadedGroups.First().EnemiesCountBeforeSpawn,
                Groups = GetGroupsForWave(loadedGroups)
            };
        }
        
        private List<GroupConfig> GetGroupsForWave(List<LoadedGroupConfig> loadedGroups)
        {
            return loadedGroups.Select(it => new GroupConfig
            {
                Count = it.Count,
                EnemyId = it.EnemyId,
                SpawnPointId = it.SpawnPointId,
                SpawnTime = it.SpawnTime
            }).ToList();
        }
    }
}