namespace Feofun.UI.Message
{
    public readonly struct ScreenSwitchMessage
    {
        public readonly string ScreenName;

        public ScreenSwitchMessage(string screenName)
        {
            ScreenName = screenName;
        }
    }
}