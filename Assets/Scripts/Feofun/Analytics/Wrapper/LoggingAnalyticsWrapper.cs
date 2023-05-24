using System.Collections.Generic;
using System.Linq;
using App.Analytics;
using Logger.Extension;
using UnityEngine.Profiling;

namespace Feofun.Analytics.Wrapper
{
    public class LoggingAnalyticsWrapper : IAnalyticsImpl
    {
        private bool _enabled;
        
        public void Init()
        {
#if UNITY_EDITOR            
            _enabled = true;
#endif            
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            if (!_enabled) return;
            Profiler.BeginSample("LoggingEvent");
            this.Logger().Info($"Event: {eventName}, Params: {DictionaryToString(eventParams)}");
            Profiler.EndSample();
        }

        private static string DictionaryToString(Dictionary<string, object> dict)
        {
            if (dict == null) return "<null>";
            var strings = dict.Select(it => $"{it.Key}:{it.Value}");
            return string.Join("\n", strings);
        }
    }
}