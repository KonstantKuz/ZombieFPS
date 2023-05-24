using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace App.Items.Config
{
    public class ItemModifiersConfigCollection : ILoadableConfig
    {
        private IReadOnlyDictionary<string, IReadOnlyList<ItemModifierConfig>> _modifiers;

        public void Load(Stream stream)
        {
            _modifiers = new CsvSerializer().ReadNestedTable<ItemModifierConfig>(stream)
                .ToDictionary(it => it.Key,
                    it => it.Value);
        }

        public bool ContainsModifiers(string itemId) => _modifiers.ContainsKey(itemId);

        public IReadOnlyList<ItemModifierConfig> GetModifiers(string itemId)
        {
            if (!_modifiers.ContainsKey(itemId)) {
                throw new NullReferenceException($"No ItemModifierConfigs for id {itemId} in ItemModifiersConfigCollection");
            }

            return _modifiers[itemId];
        }
    }
}