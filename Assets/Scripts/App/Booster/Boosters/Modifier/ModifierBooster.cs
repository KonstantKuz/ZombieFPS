using App.Items.Config;
using App.Player.Service;
using Feofun.Modifiers.Config;
using Zenject;

namespace App.Booster.Boosters.Modifier
{
    public class ModifierBooster : BoosterBase
    {
        [Inject] private PlayerService _playerService;
        private readonly ModifierBoosterConfig _config;

        public ModifierConfig ModifierConfig => _config.ModifierConfig;
        public ItemModifierTarget ModifierTarget => _config.ModifierTarget;
        
        public ModifierBooster(ModifierBoosterConfig config) : base(config)
        {
            _config = config;
        }
        
        public override void Start()
        {
            _playerService.OnEquipmentChanged();
        }

        public override void Term()
        {
            _playerService.OnEquipmentChanged();
        }
    }
}