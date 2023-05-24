using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Unit.Service
{
    public class UnitService
    {
        private readonly Dictionary<LayerMask, HashSet<Unit>> _units = new Dictionary<LayerMask, HashSet<Unit>>();
      
        public void Add(Unit unit)
        {
            var layer = unit.LayerMaskProvider.Layer;
            if (!_units.ContainsKey(layer)) {
                _units[layer] = new HashSet<Unit>();
            }
            _units[layer].Add(unit);
        }

        public void Remove(Unit unit)
        {
            if (!_units.ContainsKey(unit.LayerMaskProvider.Layer)) return;
            _units[unit.LayerMaskProvider.Layer].Remove(unit);
        }
        
        public IEnumerable<Unit> GetUnitsOfLayer(LayerMask layer)
        {
            return _units.ContainsKey(layer) ? _units[layer] : Enumerable.Empty<Unit>();
        }
    }
}