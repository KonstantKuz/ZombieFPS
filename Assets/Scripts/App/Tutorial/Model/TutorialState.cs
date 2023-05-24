using System.Collections.Generic;
using App.Tutorial.Scenario;

namespace App.Tutorial.Model
{
    public class TutorialState
    {
        public readonly Dictionary<TutorialScenarioId, ScenarioState> Scenarios = new();
    }
}