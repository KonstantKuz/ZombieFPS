﻿using Feofun.ObjectPool.Service;
using Feofun.ObjectPool.Wrapper;
using UnityEngine;
using Zenject;

namespace Feofun.ObjectPool.Installer
{
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] private DiObjectPoolWrapper _diObjectPoolWrapper;

        public void Install(DiContainer container)
        {
            container.Bind<PoolManager>().AsSingle().WithArguments(_diObjectPoolWrapper);
            container.Bind<PoolPreparer>().AsSingle();
        }
    }
}