using UnityEngine;

namespace App.MiniMap.Component
{
    public interface IMiniMapMarker
    {
        MapMarkerType MarkerType { get; }
        Vector3 WorldPosition { get; }
    }
}