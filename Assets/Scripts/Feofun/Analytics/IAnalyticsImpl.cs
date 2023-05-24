using System.Collections.Generic;
using JetBrains.Annotations;

namespace Feofun.Analytics
{
    public interface IAnalyticsImpl
    {
        void Init();
        void ReportEventWithParams(string eventName, [CanBeNull] Dictionary<string, object> eventParams, IEventParamProvider eventParamProvider);
    }
}