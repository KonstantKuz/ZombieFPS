using App.Enemy.Dismemberment.Model;
using Feofun.Util.SerializableDictionary;
using Feofun.World.Model;
using UnityEngine;

namespace App.Enemy.Dismemberment.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DismembermentVfxConfig", fileName = "DismembermentVfxConfig")]
    public class DismembermentVfxConfig : ScriptableObject
    {
        [SerializeField] private WorldObject _defaultDestroyVfx;
        [SerializeField] private SerializableDictionary<BodyMemberType, WorldObject> _destroyFragmentVfxMap;
        
        public WorldObject GetDestroyVfx(BodyMemberType bodyMemberType)
        {
            return _destroyFragmentVfxMap.ContainsKey(bodyMemberType) ? _destroyFragmentVfxMap[bodyMemberType] : _defaultDestroyVfx;
        }
    }
}
