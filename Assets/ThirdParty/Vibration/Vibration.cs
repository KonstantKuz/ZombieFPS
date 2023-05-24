////////////////////////////////////////////////////////////////////////////////
//
// @author Benoît Freslon @benoitfreslon
// https://github.com/BenoitFreslon/Vibration
// https://benoitfreslon.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public static partial class Vibration
{
    private static bool _initialized;

    public static void Init()
    {

        if (_initialized) return;
        if (Application.isMobilePlatform)
        {
            InitPlatform();
        }

        _initialized = true;
    }
}
