﻿using Feofun.Core.Init;
using Zenject;

namespace Feofun.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private ABTest _abTest;        

        protected override void Run()
        {
            _abTest.Reload();
            Next();
        }
    }
}