using UnityEngine;

namespace App.Repository
{
    public class ClearPlayerPrefsOnBranchChanged : MonoBehaviour
    {
        private const string PLAYER_PREFS_KEY = "ProjectBranch";
        [SerializeField] private string _projectBranch;

        private void Awake()
        {
            var previousBranch = PlayerPrefs.GetString(PLAYER_PREFS_KEY) ?? "";
            if (previousBranch != _projectBranch)
            {
                PlayerPrefs.DeleteAll();
            }
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, _projectBranch);
        }
    }
}