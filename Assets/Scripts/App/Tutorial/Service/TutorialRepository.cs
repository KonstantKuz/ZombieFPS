using App.Tutorial.Model;
using Feofun.Repository;

namespace App.Tutorial.Service
{
    public class TutorialRepository: LocalPrefsSingleRepository<TutorialState>
    {
        public TutorialRepository() : base("tutorial_v1")
        {
        }
    }
}