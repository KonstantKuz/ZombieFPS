namespace App.Tutorial.Scenario
{
    public class TutorialStepReporter
    {
        private readonly Analytics.Analytics _analytics;
        private readonly string _scenarioId;
        private int _step;
        
        public TutorialStepReporter(Analytics.Analytics analytics, TutorialScenarioId scenarioId)
        {
            _analytics = analytics;
            _scenarioId = scenarioId.ToString();
        }

        public void Report()
        {
            _step++;
            _analytics.ReportTutorial(_scenarioId, _step);
        }
    }
}