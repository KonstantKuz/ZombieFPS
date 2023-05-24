using System.Collections.Generic;
using System.Linq;
using App.Unit.Component.Target;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Unit.Service
{
    public class TargetService
    {
        private readonly Dictionary<LayerMask, HashSet<ITarget>> _targets = new Dictionary<LayerMask, HashSet<ITarget>>();
      
        public void Add(ITarget target)
        {
            if (!_targets.ContainsKey(target.Layer)) {
                _targets[target.Layer] = new HashSet<ITarget>();
            }
            _targets[target.Layer].Add(target);
        }

        public void Remove(ITarget target)
        {
            _targets[target.Layer].Remove(target);
        }
        
        public IEnumerable<ITarget> GetTargetsOfLayerMask(LayerMask layerMask)
        {
            if (_targets.ContainsKey(layerMask))
            {
                return _targets[layerMask];
            }

            return _targets.Where(it => layerMask.Intersects(it.Key))
                .SelectMany(it => it.Value);
        }

        public IEnumerable<ITarget> GetTargetsInRadius(Vector3 from, LayerMask layer, float radius)
        {
            foreach (var target in GetTargetsOfLayerMask(layer))
            {
                var distance = Vector3.Distance(from, target.Root.position);
                if (distance > radius) continue;
                yield return target;
            }
        }
        public IEnumerable<ITarget> GetOrderedTargetsInRadius(Vector3 from, LayerMask layer, float radius)
        {
            return GetTargetsInRadius(from, layer, radius)
                .OrderBy(it => Vector3.Distance(it.Root.position, from));
        }

        [CanBeNull]
        public ITarget FindClosestTargetOfLayer(LayerMask layer, Vector3 pos)
        {
            return GetTargetsOfLayerMask(layer).OrderBy(it => Vector3.Distance(it.Root.position, pos)).FirstOrDefault();
        }
    }
}