using System;

namespace Feofun.UI.Message
{
    public readonly struct DialogOpenedMessage
    {
        public readonly Type DialogType;

        public DialogOpenedMessage(Type dialogType)
        {
            DialogType = dialogType;
        }
    }
}