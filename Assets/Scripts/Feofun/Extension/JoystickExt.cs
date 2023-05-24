using UnityEngine;

namespace Feofun.Extension
{
    public static class JoystickExt
    {
        public static void Attach(this Joystick joystick, Transform parent)
        {
            joystick.transform.SetParent(parent);
            joystick.transform.SetAsFirstSibling();
        }
    }
}