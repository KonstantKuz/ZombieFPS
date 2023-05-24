using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.Booster.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Booster/BoosterConfigCollection", fileName = "BoosterConfigCollection")]
    public class BoosterConfigCollection: ScriptableObject
    {
        [SerializeField] 
        private List<BoosterConfigBase> _configs;
        
        public void BindConfigs(DiContainer container)
        {
            foreach (var config in _configs)
            {
                container.Bind<BoosterConfigBase>().WithId(config.BoosterId).FromInstance(config);
            }
        }
    }
}