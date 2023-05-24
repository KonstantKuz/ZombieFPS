using System;
using App.MiniMap.Component;
using App.MiniMap.Service;
using Feofun.Core.Update;
using Feofun.Util;
using UniRx;
using UnityEngine;

namespace App.UI.Screen.World.Player.MiniMap
{
    public class MiniMapModel : IDisposable
    {
        private readonly MiniMapService _miniMapService;
        private readonly UpdateManager _updateManager;
        private readonly RectTransform _mapBounds;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
     
        private Vector3 _previousOriginPosition;
        private Vector3 _previousCenterForward;

        private Transform WorldSpaceOrigin => _miniMapService.CoordSystemCenter;
        public Vector3 RuntimeOffset { get; private set; }
        public float RuntimeAngleOffset { get; private set; }
   
        public MiniMapModel(MiniMapService miniMapService, 
            UpdateManager updateManager,
            RectTransform mapBounds, 
            Action<IMiniMapMarker> onMarkerAdded, 
            Action<IMiniMapMarker> onMarkerRemoved)
        {
            _miniMapService = miniMapService;
            _updateManager = updateManager;
            _mapBounds = mapBounds;

            miniMapService.ActiveMarkers.ForEach(onMarkerAdded);
            miniMapService.OnMarkerAdded += onMarkerAdded;
            miniMapService.OnMarkerRemoved += onMarkerRemoved;

            updateManager.StartUpdate(TrackMapOffset);
            
            Disposable.Create(() =>
            {
                miniMapService.OnMarkerAdded -= onMarkerAdded;
                miniMapService.OnMarkerRemoved -= onMarkerRemoved;
            }).AddTo(_disposable);
        }

        private void TrackMapOffset()
        {
            var previousOriginPosition = _miniMapService.FromWorldToMapPoint(_previousOriginPosition);
            var currentOriginPosition = _miniMapService.FromWorldToMapPoint(WorldSpaceOrigin.position);
            RuntimeOffset = previousOriginPosition - currentOriginPosition;
            _previousOriginPosition = WorldSpaceOrigin.position;

            RuntimeAngleOffset = Vector3.SignedAngle(WorldSpaceOrigin.forward, _previousCenterForward, Vector3.up);
            _previousCenterForward = WorldSpaceOrigin.forward;
        }

        public bool IsVisibleOnMap(Vector2 mapPoint)
        {
            return _mapBounds.rect.Contains(mapPoint);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _updateManager.StopUpdate(TrackMapOffset);
        }
    }
}