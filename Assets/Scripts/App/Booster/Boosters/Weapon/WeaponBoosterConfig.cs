using App.Booster.Config;
using UnityEngine;
using Zenject;

namespace App.Booster.Boosters.Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Booster/WeaponBoosterConfig", fileName = "WeaponBoosterConfig")]
    public class WeaponBoosterConfig : BoosterConfigBase
    {
        [SerializeField] 
        private string _weaponId;

        public string WeaponId => _weaponId;
        public override BoosterBase CreateBooster(DiContainer container)
        {
            return container.Instantiate<WeaponBooster>(new []{this});
        }
    }
}