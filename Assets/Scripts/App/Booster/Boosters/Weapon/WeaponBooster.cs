using App.Extension;
using App.Weapon.Component;
using App.Weapon.Service;
using Zenject;

namespace App.Booster.Boosters.Weapon
{
    public class WeaponBooster : BoosterBase
    {
        [Inject] private WeaponService _weaponService;
        private readonly WeaponBoosterConfig _config;
        
        public WeaponBooster(WeaponBoosterConfig config) : base(config)
        {
            _config = config;
        }
        
        public override void Start()
        {
            var weaponState = _weaponService.GetRuntimeWeaponState(_config.WeaponId);
            var clip = weaponState.Clip;
            if (!clip.IsFull()) {
                clip.Load(clip.GetAmmoCountForFullLoad());
            }
            Equip();
        }

        public void Equip()
        {
            _weaponService.EquipPermanentWeapon(_config.WeaponId);
        }
        
        public override void Term()
        {
            _weaponService.UnEquipPermanentWeapon();
        }
    }
}