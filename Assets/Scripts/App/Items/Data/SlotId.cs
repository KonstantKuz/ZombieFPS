using System;
using UnityEngine;

namespace App.Items.Data
{
    [Serializable]
    public struct SlotId : IEquatable<SlotId>
    {
        [SerializeField]
        private ItemType _type;
        [SerializeField]
        private int _index;
        public ItemType Type => _type;
        public int Index => _index;
        
        public SlotId(ItemType type, int index)
        {
            _type = type;
            _index = index;
        }

        public bool Equals(SlotId other)
        {
            return Type == other.Type && Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            return obj is SlotId other && Equals(other);
        }

        public override string ToString()
        {
            return $"{Type}-{Index + 1}";
        }
        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, Index);
        }
    }
}