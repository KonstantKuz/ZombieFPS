using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;
using JetBrains.Annotations;

namespace App.BattlePass.Config
{
    [PublicAPI]
    public class BattlePassConfigList : ILoadableConfig
    {
        public IReadOnlyList<BattlePassConfig> Items { get; private set; }

        public void Load(Stream stream)
        {
            Items = new CsvSerializer().ReadObjectArray<BattlePassConfig>(stream);
        }
 
        [CanBeNull]
        public BattlePassConfig FindConfigByLevel(int level)
        {
            return Items.FirstOrDefault(it => it.Level == level);
        }

        public int GetMaxLevel()
        {
            return Items.OrderByDescending(it => it.Level).First().Level;
        }
        
        [CanBeNull]
        public BattlePassConfig FindByLevel(int level)
        {
            var config = Items.FirstOrDefault(it => it.Level == level);
            return config;
        }
    }
}