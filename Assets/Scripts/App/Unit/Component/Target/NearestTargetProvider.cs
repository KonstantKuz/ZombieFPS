using System.Collections.Generic;
using App.Unit.Service;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Profiling;

namespace App.Unit.Component.Target
{
    public class NearestTargetProvider : ITargetProvider
    {
        private readonly TargetService _targetService;
        private readonly Unit _owner;
        private readonly float _searchDistance;
        
        public ITarget Target => Find();

        public NearestTargetProvider(TargetService targetService, Unit owner, float searchDistance)
        {
            _targetService = targetService;
            _owner = owner;
            _searchDistance = searchDistance;
        }
        
        public ITarget Find()
        {
            Profiler.BeginSample("NearestTargetProvider.Find");
            var targets = _targetService.GetTargetsOfLayerMask(_owner.LayerMaskProvider.DamageMask);
            var target = Find(targets, _owner.SelfTarget.Root.position, _searchDistance);
            Profiler.EndSample();
            return target;
        }

        [CanBeNull]
        public static ITarget Find(IEnumerable<ITarget> targets, Vector3 from, float searchDistance)
        {
            ITarget result = null;
            var minDistance = Mathf.Infinity;
            foreach (var target in targets)
            {
                if (!target.IsValid()) continue;
                var dist = Vector3.Distance(from, target.Root.position);
                if (dist >= minDistance || dist > searchDistance) continue;
                minDistance = dist;
                result = target;
            }
            
            return result;
        }
    }
}