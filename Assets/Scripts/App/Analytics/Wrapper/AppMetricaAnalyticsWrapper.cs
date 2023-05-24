using System;
using System.Collections.Generic;
using App.Session.Model;
using Feofun.Analytics;
using Feofun.Extension;
using Logger.Extension;
using UnityEngine.Profiling;

namespace App.Analytics.Wrapper
{
    public class AppMetricaAnalyticsWrapper : IAnalyticsImpl
    {
        public void Init() { }
        
        public void ReportTest()
        {
            ReportEvent("Test", new Dictionary<string, object>());
        }
        
        private void ReportEvent(string eventName, Dictionary<string, object> eventParams)
        {
            var jsonParams = eventParams.ConvertToJson();
            AppMetrica.Instance.ReportEvent(eventName, jsonParams);
            this.Logger().Debug($"Send event {eventName} with json params {jsonParams}");
        }

        public void ReportEventWithParams(string eventName, 
            Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            Profiler.BeginSample("AppMetricaReportEvent");
            ReportEvent(eventName, eventParams);
            UpdateProfileParams(eventName, eventParams, eventParamProvider);
            Profiler.EndSample();
        }

        private static void UpdateProfileParams(string eventName, Dictionary<string, object> eventParams, IEventParamProvider eventParamProvider)
        {
            if(eventName == Events.FREEZE) return;
            
            var additionalParams = RequestAdditionalParams(eventName, eventParams, eventParamProvider);
            var profile = new YandexAppMetricaUserProfile();
            var updates = new List<YandexAppMetricaUserProfileUpdate>
            {
                BuildStringAttribute(EventParams.LAST_EVENT, BuildLastEventName(eventName, eventParams)), 
                BuildStringAttribute(EventParams.LEVEL_ID, eventParams[EventParams.LEVEL_ID].ToString()),
                BuildFloatAttribute(EventParams.WINS, additionalParams[EventParams.WINS]),
                BuildFloatAttribute(EventParams.DEFEATS, additionalParams[EventParams.DEFEATS]),
                BuildFloatAttribute(EventParams.LEVEL_NUMBER, eventParams[EventParams.LEVEL_NUMBER]),
            };
            profile.ApplyFromArray(updates);
            AppMetrica.Instance.ReportUserProfile(profile);
        }
        
        private static Dictionary<string, object> RequestAdditionalParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            var additionalParams = eventParamProvider.GetParams(new[]
            {
                EventParams.WINS,
                EventParams.DEFEATS,
            });
            if (eventName == Events.LEVEL_FINISHED)
            {
                AddLevelResultToWinDefeatCount(eventParams, additionalParams);
            }

            return additionalParams;
        }

        private static void AddLevelResultToWinDefeatCount(IReadOnlyDictionary<string, object> eventParams, IDictionary<string, object> additionalParams)
        {
            additionalParams[EventParams.WINS] = Convert.ToInt32(additionalParams[EventParams.WINS]) +
                                                 ((string)eventParams[EventParams.LEVEL_RESULT] == SessionResult.Win.ToString() ? 1 : 0);
            
            additionalParams[EventParams.DEFEATS] = Convert.ToInt32(additionalParams[EventParams.DEFEATS]) +
                                                    ((string)eventParams[EventParams.LEVEL_RESULT] == SessionResult.Lose.ToString() ? 1 : 0);
        }

        private static YandexAppMetricaUserProfileUpdate BuildFloatAttribute(string name, object value)
        {
            return new YandexAppMetricaNumberAttribute(name).WithValue(Convert.ToDouble(value));
        }
        
        private static YandexAppMetricaUserProfileUpdate BuildStringAttribute(string name, string value)
        {
            return new YandexAppMetricaStringAttribute(name).WithValue(value);
        }

        private static string BuildLastEventName(string eventName, Dictionary<string,object> eventParams)
        {
            return eventName switch
            {
                Events.LEVEL_START => $"level_start_{eventParams[EventParams.LEVEL_ID]}",
                Events.LEVEL_FINISHED =>
                $"level_finished_{eventParams[EventParams.LEVEL_ID]}_{eventParams[EventParams.LEVEL_RESULT]}",
                Events.ITEM_PUT_ON =>
                $"item_put_on_{eventParams[EventParams.ITEM_ID]}_{eventParams[EventParams.SLOT_ID]}",
                Events.ITEM_PUT_OFF =>
                $"item_put_off_{eventParams[EventParams.ITEM_ID]}_{eventParams[EventParams.SLOT_ID]}",
                Events.TUTORIAL => 
                $"tutorial_{eventParams[EventParams.TUTORIAL_STEP]}_{eventParams[EventParams.TUTORIAL_SCENARIO]}",
                _ => eventName
            };
        }
    }
}