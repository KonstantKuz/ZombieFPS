using System;
using System.Collections.Generic;
using App.MiniMap.Component;
using App.MiniMap.Service;
using Feofun.Core.Update;
using Feofun.Util.SerializableDictionary;
using Feofun.Extension;
using Feofun.World.Factory.ObjectFactory;
using Feofun.World.Model;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player.MiniMap
{
    public class MiniMap : MonoBehaviour
    {
        [SerializeField] private Transform _markersRoot;
        [SerializeField] private Transform _grid;
        [SerializeField] private RectTransform _mapBounds;
        [SerializeField] private SerializableDictionary<MapMarkerType, WorldObject> _markerPrefabs;

        [Inject] private MiniMapService _miniMapService;
        [Inject] private UpdateManager _updateManager;
        [Inject(Id = ObjectFactoryType.Pool)]  private IObjectFactory _objectFactory;

        private MiniMapModel _model;
        private Dictionary<IMiniMapMarker, MiniMapMarkerView> _activeMarkers = new ();
        
        private void OnEnable()
        {
            _model = new MiniMapModel(_miniMapService, _updateManager, _mapBounds,  OnMarkerAdded, OnMarkerRemoved);
        }

        private void OnMarkerAdded(IMiniMapMarker marker)
        {
            var markerView = _objectFactory.Create<MiniMapMarkerView>(GetMarkerPrefab(marker.MarkerType).ObjectId);
            markerView.transform.SetParent(_markersRoot);
            var positionOnMap = _miniMapService.FromWorldToMapPoint(marker.WorldPosition);
            markerView.UpdatePosition(positionOnMap);
            _activeMarkers.Add(marker, markerView);
        }

        private void OnMarkerRemoved(IMiniMapMarker marker)
        {
            var view = _activeMarkers[marker];
            _activeMarkers.Remove(marker);
            _objectFactory.Destroy(view.gameObject);
        }

        private void Update()
        {
            _activeMarkers.ForEach(it => UpdateMarkerView(it.Value, it.Key));
            UpdateGrid();
        }
        
        private void UpdateMarkerView(MiniMapMarkerView markerView, IMiniMapMarker worldMarker)
        {
            var positionOnMap = _miniMapService.FromWorldToMapPoint(worldMarker.WorldPosition);
            if (!_model.IsVisibleOnMap(positionOnMap)) return;
            markerView.UpdatePosition(positionOnMap);
        }

        private void UpdateGrid()
        {
            _grid.position += _model.RuntimeOffset;
            _grid.RotateAround(_mapBounds.position, -Vector3.forward, _model.RuntimeAngleOffset);
        }
        
        private WorldObject GetMarkerPrefab(MapMarkerType markerType)
        {
            if (!_markerPrefabs.ContainsKey(markerType))
            {
                throw new NullReferenceException($"There is no marker prefab for marker type := {markerType}");
            }

            return _markerPrefabs[markerType];
        }

        private void OnDisable()
        {
            _model?.Dispose();
            _activeMarkers.Clear();
        }
    }
}
