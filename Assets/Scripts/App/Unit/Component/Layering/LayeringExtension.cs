using Feofun.Extension;
using UnityEngine;

namespace App.Unit.Component.Layering
{
    public static class LayeringExtension
    {
        public static bool IsLayerIntersectsWith(this GameObject target, LayerMask layerMask)
        {
            return target.TryGetComponent(out LayerMaskProvider targetMaskProvider) &&
                   layerMask.Intersects(targetMaskProvider.Layer);
        }
    }
}