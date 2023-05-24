using Feofun.Util.SerializableDictionary;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace App.Player.Config.Animation
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WalkAnimationConfigCollection", fileName = "WalkAnimationConfigCollection")]
    public class WalkAnimationConfigCollection : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<string, WalkAnimationConfig> _configs;

        [CanBeNull]
        public WalkAnimationConfig FindConfig(string id)
        {
            if (!_configs.ContainsKey(id)) {
                this.Logger().Error($"Can't find walkAnimationConfig for id := {id}");
                return null;
            }

            return _configs[id];
        }
    }
}