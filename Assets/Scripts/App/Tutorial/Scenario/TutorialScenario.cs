using System;
using App.Tutorial.Model;
using App.Tutorial.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Tutorial.Scenario
{
    public abstract class TutorialScenario : MonoBehaviour
    {
        private bool _isActive;
        
        [SerializeField]
        private bool _isEnabled;
        [SerializeField]
        private TutorialScenarioId _scenarioId;
        
        [Inject]
        protected TutorialService TutorialService { get; }

        public bool IsEnabled => _isEnabled;
        public TutorialScenarioId ScenarioId => _scenarioId;
        protected ScenarioState State => TutorialService.GetScenarioState(ScenarioId);
        public bool IsCompleted => State.IsCompleted;
        public bool IsActive
        {
            get => _isActive;
            protected set
            {
                _isActive = value;
                TutorialService.OnScenarioActiveStateChanged(this);
            }
        }

        public abstract void Init();
        
        protected bool IsStepCompleted(string stepName)
        {
            return State.CompletedSteps.Contains(stepName);
        }

        protected void CompleteStep(string stepName)
        {
            var state = State;
            state.CompletedSteps.Add(stepName);
            TutorialService.SetScenarioState(ScenarioId, state);
        }

        protected void CompleteScenario()
        {
            TutorialService.CompleteScenario(_scenarioId);
        }
    }
}