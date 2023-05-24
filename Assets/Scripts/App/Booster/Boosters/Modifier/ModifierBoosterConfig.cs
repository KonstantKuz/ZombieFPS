using App.Booster.Config;
using App.Items.Config;
using Feofun.Modifiers.Config;
using Feofun.Modifiers.Data;
using UnityEngine;
using Zenject;

namespace App.Booster.Boosters.Modifier
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Booster/ModifierBoosterConfig", fileName = "WeaponBoosterConfig")]
    public class ModifierBoosterConfig : BoosterConfigBase
    {
        [SerializeField] private ItemModifierTarget _modifierTarget;
        [SerializeField] private ModifierType _modifierType;
        [SerializeField] private float _modifierValue;
        [SerializeField] private string _parameterName;
        
        public ModifierConfig ModifierConfig => new ModifierConfig(_modifierType, _modifierValue, _parameterName);
        public ItemModifierTarget ModifierTarget => _modifierTarget;
        public override BoosterBase CreateBooster(DiContainer container)
        {
            return container.Instantiate<ModifierBooster>(new []{this});
        }
    }
}