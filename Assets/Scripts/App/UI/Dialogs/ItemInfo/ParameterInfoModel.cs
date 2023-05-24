namespace App.UI.Dialogs.ItemInfo
{
    public class ParameterInfoModel
    {
        public string ParamName { get; }
        public string ValueInfo { get; }

        public ParameterInfoModel(string paramName, string valueInfo)
        {
            ParamName = paramName;
            ValueInfo = valueInfo;
        }
    }
}