using System;
using App.Items.Config;

namespace App.Items.Data
{
    public class Item : IEquatable<Item>
    {
        private readonly ItemConfig _config;
        public string Id => _config.Id;
        public ItemType Type => _config.Type;

        public Item(ItemConfig config)
        {
            _config = config;
        }

        public bool Equals(Item other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_config, other._config);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return (_config != null ? _config.GetHashCode() : 0);
        }
    }
}