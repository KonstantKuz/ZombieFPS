using UnityEngine;

namespace App.Session.Model
{
    public class Session
    {
        private float _startTime;
        public string LevelId { get; }
        public float SessionTime => Time.time - _startTime;
        public int KillCount { get; private set; }
        public SessionResult? Result { get; private set; }

        public bool IsWon => Result.Value == SessionResult.Win;
        
        public Session(string levelId) => LevelId = levelId;

        public static Session Create(string levelId) => new Session(levelId);

        public void OnStart()
        {
            _startTime = Time.time;
        }

        public void AddKill()
        {
            KillCount++;
        }

        public void SetResult(SessionResult result)
        {
            Result = result;
        }
    }
}