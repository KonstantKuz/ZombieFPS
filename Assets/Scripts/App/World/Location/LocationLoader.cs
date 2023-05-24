using System;
using System.Collections;
using System.Linq;
using App.Core;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace App.World.Location
{
    public class LocationLoader
    {
        [Inject] private SceneService _sceneService;   
        [Inject] private ICoroutineRunner _coroutineRunner;

        private Coroutine _loadCoroutine;
        private Action<Location> _onLoaded;

        public Location CurrentLocation { get; private set; }

        public void Load(string id, Action<Location> onLoaded)
        {
            Dispose();
            _onLoaded = onLoaded;
            _loadCoroutine = _coroutineRunner.StartCoroutine(LoadLocation(id));
        } 
        public IEnumerator Unload() => _sceneService.UnloadAdditiveScene();

        private IEnumerator LoadLocation(string id)
        {
            Profiler.BeginSample("[LocationLoader] Load additive location"); 

            yield return _sceneService.LoadAdditiveScene(id);
            var scene = _sceneService.CurrentAdditiveScene;
            if (scene == null) {
                throw new NullReferenceException("Location scene loading error");
            }

            CurrentLocation = scene.Value.GetRootGameObjects()
                .First(it => it.GetComponent<Location>() != null)
                .RequireComponent<Location>();
            _onLoaded?.Invoke(CurrentLocation);
            Profiler.EndSample();
        }
        
        private void Dispose()
        {
            if (_loadCoroutine == null) return;
            _coroutineRunner.StopCoroutine(_loadCoroutine);
            _loadCoroutine = null;
            CurrentLocation = null;
        }
        
    }
}