using Feofun.Repository;

namespace App.Settings
{
    public class SettingsRepository: LocalPrefsSingleRepository<SettingsData>
    {
        public SettingsRepository() : base("Settings_v0")
        {
        }
    }
}