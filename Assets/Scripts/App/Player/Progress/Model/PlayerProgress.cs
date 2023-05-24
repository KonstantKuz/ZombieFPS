namespace App.Player.Progress.Model
{
    public class PlayerProgress
    {
        public int GameCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount => GameCount - WinCount;
        public int Kills { get; set; }
        public int PlayerLevel => WinCount + 1;
        public int LevelTry { get; set; }
        public bool IsLastLevelWon { get; set; }

        public static PlayerProgress Create() => new PlayerProgress();
    }
}