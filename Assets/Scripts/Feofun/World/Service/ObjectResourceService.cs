using System.Collections.Generic;
using Feofun.World.Model;
using UnityEngine;
using UnityEngine.Profiling;

namespace Feofun.World.Service
{
    public class ObjectResourceService
    {
        private const string OBJECT_PREFABS_PATH_ROOT = "Content/";
        
        private readonly Dictionary<string, WorldObject> _prefabs = new Dictionary<string, WorldObject>();

        public ObjectResourceService()
        {
            Profiler.BeginSample("[ObjectResourceService] Load prefabs");
            
            LoadPrefabsObjects();
            
            Profiler.EndSample();
        }
        private void LoadPrefabsObjects()
        {
            var worldObjects = Resources.LoadAll<WorldObject>(OBJECT_PREFABS_PATH_ROOT);
            foreach (var worldObject in worldObjects) {
                _prefabs.Add(worldObject.ObjectId, worldObject);
            }
        }
        public IEnumerable<WorldObject> GetAllPrefabs() => _prefabs.Values;
        public WorldObject GetPrefab(string objectId)
        {
            if (!_prefabs.ContainsKey(objectId)) {
                throw new KeyNotFoundException($"WorldObject prefab not found with objectId:= {objectId}");
            }
            return _prefabs[objectId];
        }
        
        public WorldObject FindPrefab(string objectId)
        {
            return !_prefabs.ContainsKey(objectId) ? null : _prefabs[objectId];
        }
    }
}