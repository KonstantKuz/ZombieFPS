using System.Collections.Generic;
using System.Linq;
using App.Tutorial.Model;
using App.Tutorial.Scenario;
using App.UI.Tutorial;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Tutorial.Service
{
    public class TutorialService : MonoBehaviour
    {
        [SerializeField] private TutorialUiTools _uiTools;
        
        private List<TutorialScenario> _scenarios;
        
        [Inject] private TutorialRepository _repository;

        private List<TutorialScenario> Scenarios => _scenarios ??= GetComponentsInChildren<TutorialScenario>().ToList();
        private TutorialState State => _repository.Get() ?? new TutorialState();
        public TutorialUiTools UiTools => _uiTools;
        public bool IsAnyScenarioIsActive => Scenarios.Any(it => it.IsActive);
        public ReactiveProperty<bool> IsAnyScenarioActiveAsObservable { get; } = new ReactiveProperty<bool>(false);
        
        public void Init()
        {
            Scenarios.ForEach(it =>
            {
                if (it.IsEnabled && !it.IsCompleted)
                {
                    it.Init();
                }
            });
        }

        public void OnScenarioActiveStateChanged(TutorialScenario scenario)
        {
            IsAnyScenarioActiveAsObservable.Value = Scenarios.Any(it => it.IsActive);
        }
        
        public ScenarioState GetScenarioState(TutorialScenarioId scenarioId)
        {
            if (!State.Scenarios.ContainsKey(scenarioId))
            {
                SetScenarioState(scenarioId, new ScenarioState());
            }
            
            return State.Scenarios[scenarioId];
        }

        public void SetScenarioState(TutorialScenarioId scenarioId, ScenarioState scenarioState)
        {
            var state = State;
            state.Scenarios[scenarioId] = scenarioState;
            _repository.Set(state);
        }

        public void CompleteScenario(TutorialScenarioId scenarioId)
        {
            var state = State;
            state.Scenarios[scenarioId].IsCompleted = true;
            _repository.Set(state);
        }
    }
}