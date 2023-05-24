using System;
using System.Collections.Generic;
using App.MiniMap.Component;
using App.MiniMap.Config;
using App.Player.Service;
using Feofun.Extension;
using Feofun.World;
using UnityEngine;
using Zenject;

namespace App.MiniMap.Service
{
    public class MiniMapService : IWorldScope
    {
        private readonly List<IMiniMapMarker> _activeMarkers = new ();

        [Inject] private PlayerService _playerService;
        [Inject] private MiniMapConfig _miniMapConfig;

        public Transform CoordSystemCenter => _playerService.RequirePlayer().transform;
        
        public List<IMiniMapMarker> ActiveMarkers => _activeMarkers;

        public event Action<IMiniMapMarker> OnMarkerAdded;
        public event Action<IMiniMapMarker> OnMarkerRemoved;

        public void OnWorldSetup() { }
        public void OnWorldCleanUp() => _activeMarkers.Clear();

        public void AddMarker(IMiniMapMarker marker)
        {
            _activeMarkers.Add(marker);
            OnMarkerAdded?.Invoke(marker);
        }

        public void RemoveMarker(IMiniMapMarker marker)
        {
            _activeMarkers.Remove(marker);
            OnMarkerRemoved?.Invoke(marker);
        }

        public Vector2 FromWorldToMapPoint(Vector3 worldPosition)
        {
            return CoordSystemCenter.InverseTransformPoint(worldPosition).ToVector2XZ() * _miniMapConfig.MapScale;
        }
    }
}