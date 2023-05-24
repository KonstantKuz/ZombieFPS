using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feofun.Extension
{
    public static class LayerExt
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool Intersects(this LayerMask mask, LayerMask targetMask)
        {
            return (mask.value & targetMask.value) != 0;
        }
        
        public static IEnumerable<LayerMask> GetMaskLayers(this LayerMask mask)
        {
            var bitmask = mask.value;
            for (int i = 0; i < 32; i++)
            {
                var intersection = (1 << i) & bitmask;
                if (intersection != 0)
                {
                    yield return 1 << i;
                }
            }
        }

        public static LayerMask GetLayerCollisionMask(int layer)
        {
            int layerMask = 0;
            for (int i = 0; i < 32 ; i++) {
                if(!Physics.GetIgnoreLayerCollision(layer, i))  {
                    layerMask |= 1 << i;
                }
            }

            return layerMask;
        }
    }
}