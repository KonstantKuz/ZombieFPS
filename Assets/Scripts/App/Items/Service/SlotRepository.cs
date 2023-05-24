using System.Collections.Generic;
using App.Items.Data;
using Feofun.Repository;

namespace App.Items.Service
{
    public class SlotRepository : LocalPrefsSingleRepository<Dictionary<string, SlotKit>>
    {
        public SlotRepository() : base("Slot_v0")
        {
            
        }

    }
}