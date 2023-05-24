using System.Collections.Generic;
using Logger.Extension;
using UnityEngine;

namespace Feofun.Core.Init
{
    public class AppInitSequence : MonoBehaviour
    {
        private readonly Queue<AppInitStep> _steps = new();

        public void AddStep<T>() where T : AppInitStep
        {
            var loadable = AppContext.Container.Instantiate<T>();
            _steps.Enqueue(loadable);
        }
        public void AddStep<T>(IEnumerable<object> extraArgs) where T : AppInitStep
        {
            var step = AppContext.Container.Instantiate<T>(extraArgs);
            _steps.Enqueue(step);
        }

        public void Next()
        {
            if (_steps.Count == 0) {
                Destroy(this);
                return;
            }
            var step = _steps.Dequeue();
            this.Logger().Debug($"AppInitSequence run step= {step.GetType().Name}");
            step.Run(Next);
        }
    }
}