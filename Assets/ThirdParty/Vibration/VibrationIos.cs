#if UNITY_IOS

using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

public static partial class Vibration
{
    [DllImport("__Internal")]
    private static extern bool _HasVibrator();

    [DllImport("__Internal")]
    private static extern void _Vibrate();

    [DllImport("__Internal")]
    private static extern void _VibratePop();

    [DllImport("__Internal")]
    private static extern void _VibratePeek();

    [DllImport("__Internal")]
    private static extern void _VibrateNope();

    [DllImport("__Internal")]
    private static extern void _impactOccurred(string style);

    [DllImport("__Internal")]
    private static extern void _notificationOccurred(string style);

    [DllImport("__Internal")]
    private static extern void _selectionChanged();

    [DllImport("__Internal")]
    private static extern void _VibrateWithParam(long time);

    [DllImport("__Internal")]
    private static extern void _VibrateInit();

    [DllImport("__Internal")]
    private static extern bool _IsHapticSupported();

    private static bool _isHapticSupported;

    public static void Vibrate(long time)
    {
        Assert.IsTrue(_initialized);
        if (!Application.isMobilePlatform) return;
        if (_isHapticSupported)
        {
            _VibrateWithParam(time);
        } else
        {
            VibrateWithoutHaptic(time);
        }
    }

    public static void VibrateWithoutHaptic(long time)
    {
        if (time < 500)
        {
            _VibratePop();
        }
        else
        {
            _VibratePeek();
        }
    }

    public static void InitPlatform()
    {
        _VibrateInit();
        _isHapticSupported = _IsHapticSupported();
    }

    public static void Cancel()
    {
        Assert.IsTrue(_initialized);
    }
}

#endif
