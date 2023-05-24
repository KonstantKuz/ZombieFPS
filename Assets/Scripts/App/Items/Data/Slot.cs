using JetBrains.Annotations;

namespace App.Items.Data
{
    public class Slot
    {
        public SlotId SlotId { get; }

        [CanBeNull]
        public string ItemId  { get; }
        
        public bool IsEmpty => ItemId == null;
        public ItemType Type => SlotId.Type;
        public int Index => SlotId.Index;
        
        public Slot(SlotId slotId, [CanBeNull] string itemId = null)
        {
            SlotId = slotId;
            ItemId = itemId;
        }
    }
}