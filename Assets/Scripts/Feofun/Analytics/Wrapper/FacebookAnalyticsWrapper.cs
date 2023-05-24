using System.Collections.Generic;
using Facebook.Unity;
using Logger.Extension;
using UnityEngine.Profiling;

namespace Feofun.Analytics.Wrapper
{
    public class FacebookAnalyticsWrapper : IAnalyticsImpl
    {
        public void Init() {}

        private void LogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
        {
            if (!FB.IsInitialized)
            {
                //TODO: store events while fb sdk not initialized and send them after initialization
                this.Logger().Warn($"Facebook analytics event {logEvent} is lost, cause facebook sdk is not ready yet");
                return;
            }
            Profiler.BeginSample("FacebookEvent");
            FB.LogAppEvent(logEvent, valueToSum, parameters);
            Profiler.EndSample();
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams, IEventParamProvider eventParamProvider)
        {
            LogEvent(eventName, null, eventParams);
        }
    }
}