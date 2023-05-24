using App.Level.Service;

namespace App.Session
{
    public class SessionBuilder
    {
        public static Model.Session Build(LevelIdService levelIdService)
        {
            return Model.Session.Create(levelIdService.CurrentLevelId);
        }
    }
}