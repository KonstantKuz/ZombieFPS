using System.Collections.Generic;
using System.Linq;
using Feofun.Core;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Feofun.World
{
    public class World : RootContainer
    {
        private const string SPAWN_CONTAINER_NAME = "Spawn";
        private const string POOL_CONTAINER_NAME = "Pool";

        public bool IsPaused => Time.timeScale.IsZero();
        public Transform SpawnContainer => GetContainer(SPAWN_CONTAINER_NAME);
        public Transform PoolContainer => GetContainer(POOL_CONTAINER_NAME);

        private int _pauseCounter = 0;
        
        public void Pause() 
        {
            if (_pauseCounter == 0)
            {
                Time.timeScale = 0;
                AudioListener.pause = true; //TODO: split sounds in two groups: ui and world. And pause only world sounds here
            }

            _pauseCounter++;
        }

        public void UnPause()
        {
            if (_pauseCounter == 0) return;
            _pauseCounter--;
            if (_pauseCounter > 0) return;
            AudioListener.pause = false;
            Time.timeScale = 1;
        }

        public void Setup() 
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldSetup());
        }

        public void CleanUp()
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldCleanUp());
        }

        private IEnumerable<T> GetAllOf<T>()
        {
            return GetDISubscribers<T>().Union(GetChildrenSubscribers<T>());
        }

        private static List<T> GetDISubscribers<T>() => AppContext.Container.ResolveAll<T>();
    }
}