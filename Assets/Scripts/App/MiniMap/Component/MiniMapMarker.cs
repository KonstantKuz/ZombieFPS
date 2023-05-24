using App.MiniMap.Service;
using UnityEngine;
using Zenject;

namespace App.MiniMap.Component
{
    public abstract class MiniMapMarker : MonoBehaviour, IMiniMapMarker
    {
        [SerializeField] private MapMarkerType _mapMarkerType;

        [Inject] private MiniMapService _miniMapService;

        public MapMarkerType MarkerType => _mapMarkerType;
        public Vector3 WorldPosition => transform.position;

        protected void AddToMap()
        {
            _miniMapService.AddMarker(this);
        }

        protected void RemoveFromMap()
        {
            _miniMapService.RemoveMarker(this);
        }
    }
}