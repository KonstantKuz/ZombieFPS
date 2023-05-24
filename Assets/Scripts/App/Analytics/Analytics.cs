using System.Collections.Generic;
using Feofun.Analytics;
using Feofun.Extension;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine.Profiling;
using Zenject;

namespace App.Analytics
{
    [PublicAPI]
    public class Analytics :  IAnalytics
    {
        [Inject] 
        private IEventParamProvider _eventParamProvider;

        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            this.Logger().Info("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            var eventParams = _eventParamProvider.GetParams(new[] {
                EventParams.TEST,
            });
            ReportEventToAllImpls(Events.TEST_EVENT, eventParams);
        }
        
        public void ReportAdRewardedRequested(string placementId) => ReportAdRewardedEvent(Events.AD_REWARDED_REQUESTED, placementId);

        public void ReportAdRewardedShown(string placementId) => ReportAdRewardedEvent(Events.AD_REWARDED_SHOWN, placementId);

        public void ReportAdRewardedNotShown(string placementId) => ReportAdRewardedEvent(Events.AD_REWARDED_NOT_SHOWN, placementId);


        public void ReportLevelStart()
        {
            ReportEventToAllImpls(Events.LEVEL_START, GetBaseParams());
        }
        
        public void ReportLevelFinished()
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_RESULT,
                EventParams.TIME_SINCE_LEVEL_START,
                EventParams.ENEMIES_KILLED,
                EventParams.PLAYER_HEALTH,
                EventParams.ROUND_NUMBER,
                EventParams.WAVE_NUMBER,
            });
            eventParams = GetBaseParams().UnionWith(eventParams).UnionWith(GetFpsParams());
            ReportEventToAllImpls(Events.LEVEL_FINISHED, eventParams);
        }

        public void ReportItemPutOn(string itemId, string slotId)
        {
            ReportItemAction(Events.ITEM_PUT_ON, itemId, slotId);
        }

        public void ReportItemPutOff(string itemId, string slotId)
        {
            ReportItemAction(Events.ITEM_PUT_OFF, itemId, slotId);
        }

        public void ReportTutorial(string scenarioId, int stepId)
        {
            var eventParams = new Dictionary<string, object>
            {
                {EventParams.TUTORIAL_STEP, stepId},
                {EventParams.TUTORIAL_SCENARIO, scenarioId},
            };
            eventParams = GetBaseParams().UnionWith(eventParams);
            ReportEventToAllImpls(Events.TUTORIAL, eventParams);
        }

        public void ReportReload()
        {
            var eventParams = _eventParamProvider.GetParams(new []
            {
               EventParams.WAVE_NUMBER,
               EventParams.ROUND_NUMBER,
               EventParams.LEVEL_NUMBER,
               EventParams.LEVEL_ID,
               EventParams.LEVEL_TRY,
            });
            
            ReportEventToAllImpls(Events.WEAPON_RELOAD, eventParams);
        }
        
        public void ReportRoundStart()
        {
            var eventParams = GetLevelParams().UnionWith(_eventParamProvider.GetParams(new[]
            {
                EventParams.ROUND_NUMBER
            }));
            ReportEventToAllImpls(Events.ROUND_START, eventParams);
        }

        public void ReportRoundFinish()
        {
            var eventParams = GetLevelParams().UnionWith(_eventParamProvider.GetParams(new[]
            {
                EventParams.WAVE_NUMBER,
                EventParams.ROUND_NUMBER,
                EventParams.ROUND_RESULT,
                EventParams.ROUND_TIME,
                EventParams.AVERAGE_FPS
            }));
            ReportEventToAllImpls(Events.ROUND_FINISH, eventParams);
        }

        public void ReportWaveStart()
        {
            var eventParams = GetLevelParams().UnionWith(_eventParamProvider.GetParams(new[]
            {
                EventParams.ROUND_NUMBER,
                EventParams.WAVE_NUMBER,
            }));
            ReportEventToAllImpls(Events.WAVE_START, eventParams);
        }

        private Dictionary<string, object> GetBaseParams()
        {
            return GetLevelParams().UnionWith(
                _eventParamProvider.GetParams(new[]
            {
                EventParams.WEAPON,
                EventParams.EQUIPMENT,
            }));
        }

        private Dictionary<string, object> GetLevelParams()
        {
            return _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_ID,
                EventParams.LEVEL_TRY,
            });
        }
        
        private Dictionary<string, object> GetFpsParams()
        {
            return _eventParamProvider.GetParams(new[]
            {
                EventParams.AVERAGE_FPS, 
                EventParams.CRITICAL_FPS_PERCENT, 
            });
        }

        public void ReportWeaponSwitch(string itemId)
        {
            Profiler.BeginSample("ReportWeaponSwitch");
            
            var eventParams = new Dictionary<string, object>
            {
                {EventParams.ITEM_ID, itemId}
            };
            eventParams = GetBaseParams().UnionWith(eventParams);
            ReportEventToAllImpls(Events.WEAPON_SWITCH, eventParams);
            
            Profiler.EndSample();
        }
        
        private void ReportItemAction(string eventName, string itemId, string slotId)
        {
            var eventParams = new Dictionary<string, object>
            {
                {EventParams.ITEM_ID, itemId},
                {EventParams.SLOT_ID, slotId},
            };
            eventParams = GetBaseParams().UnionWith(eventParams);
            ReportEventToAllImpls(eventName, eventParams);
        }
        
        private void ReportAdRewardedEvent(string eventName, string placementId)
        {
            var eventParams = new Dictionary<string, object>
            {
                {EventParams.REWARDED_PLACEMENT_ID, placementId},
            };
            eventParams = GetLevelParams().UnionWith(eventParams);
            ReportEventToAllImpls(eventName, eventParams);
        }    
        
        private void ReportEventToAllImpls(string eventName, Dictionary<string, object> eventParams)
        {
            Profiler.BeginSample("AnalyticsReportAllImpl");

            this.Logger().Debug($"Send event {eventName} with params {string.Join(":", eventParams)}");
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(eventName, eventParams, _eventParamProvider);
            }
            
            Profiler.EndSample();
        }
    }
}