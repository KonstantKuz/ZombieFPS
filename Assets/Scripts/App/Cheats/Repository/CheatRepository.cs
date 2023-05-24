using App.Cheats.Data;
using Feofun.Repository;

namespace App.Cheats.Repository
{
    public class CheatRepository : LocalPrefsSingleRepository<CheatSettings>
    {
        public CheatRepository() : base("CheatSettings")
        {
        }
    }
}