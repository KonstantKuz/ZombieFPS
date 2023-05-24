using System.Collections.Generic;
using Feofun.Repository;

namespace App.Items.Service
{
    public class InventoryRepository : LocalPrefsSingleRepository<List<string>>
    {
        public InventoryRepository() : base("Inventory_v0")
        {
            
        }

    }
}