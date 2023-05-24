using App.Player.Progress.Model;
using UniRx;

namespace App.Player.Progress.Service
{
    public class PlayerProgressService
    {
        private readonly PlayerProgressRepository _repository;
        
        private readonly IntReactiveProperty _gameCount; 
       
        public IReadOnlyReactiveProperty<int> GameCount => _gameCount;
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(PlayerProgressRepository repository)
        {
            _repository = repository;
            _gameCount = new IntReactiveProperty(Progress.GameCount);
        }

        public void OnSessionFinished(bool isWon)
        {
            var progress = Progress;
            progress.GameCount++;
            progress.LevelTry++;
            if (isWon) {
                progress.WinCount++;
                progress.LevelTry = 0;
            }

            progress.IsLastLevelWon = isWon; 
            SetProgress(progress);
        }
        
        private void SetProgress(PlayerProgress progress)
        {
            _repository.Set(progress);
            _gameCount.Value = progress.GameCount;
        }
        
        public void AddKill()
        {
            var progress = Progress;
            progress.Kills++;
            SetProgress(progress); 
        }
    }
}