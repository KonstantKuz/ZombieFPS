using App.UI.Screen.Debriefing.Model;
using UnityEngine;

namespace App.UI.Screen.Debriefing.View
{
    public class SessionResultPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        public void Init(ResultPanelModel model)
        {
            _winPanel.SetActive(model.IsWin);     
            _losePanel.SetActive(!model.IsWin);
        }
    }
}
