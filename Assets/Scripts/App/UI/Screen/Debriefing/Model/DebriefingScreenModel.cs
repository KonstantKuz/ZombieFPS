
namespace App.UI.Screen.Debriefing.Model
{
    public class DebriefingScreenModel
    {
        public readonly ResultPanelModel ResultPanelModel;

        public DebriefingScreenModel(bool isWin)
        {
            ResultPanelModel = new ResultPanelModel(isWin);
        }
    }
}