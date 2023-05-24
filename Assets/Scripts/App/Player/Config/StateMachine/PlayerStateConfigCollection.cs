using System.Collections.Generic;
using System.Linq;
using App.Player.Component.StateMachine;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace App.Player.Config.StateMachine
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerStateConfigCollection", fileName = "PlayerStateConfigCollection")]
    public class PlayerStateConfigCollection : ScriptableObject
    {
        [SerializeField]
        private List<StateConfigBase> _configs;

        private Dictionary<PlayerState, StateConfigBase> _configsMap;
        
        private Dictionary<PlayerState, StateConfigBase> ConfigsMap =>
            _configsMap ??= _configs.ToDictionary(it => it.State, it => it); 
        
        [CanBeNull]
        public T FindConfig<T>(PlayerState state) where T: StateConfigBase
        {
            if (!ConfigsMap.ContainsKey(state)) {
                this.Logger().Error($"Can't find stateConfig for state := {state}");
                return default;
            }

            return (T) ConfigsMap[state];
        }
    }
}