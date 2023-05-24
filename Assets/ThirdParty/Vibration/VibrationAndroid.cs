#if UNITY_ANDROID

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public static partial class Vibration
{
    private static AndroidJavaClass _unityPlayer;
    private static AndroidJavaObject _currentActivity;
    private static AndroidJavaObject _vibrator;
    private static AndroidJavaObject _context;

    private static AndroidJavaClass _vibrationEffect;

    private static bool IsAndroidVersionMoreThan26 => _vibrationEffect != null;
    
    public static void Vibrate(long time)
    {
        Assert.IsTrue(_initialized);
        if (!Application.isMobilePlatform) return;
        VibrateAndroid(time);
    }

    private static void InitPlatform()
    {
        if (!Application.isMobilePlatform) return;
        
        _unityPlayer = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" );
        _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject> ( "currentActivity" );
        _vibrator = _currentActivity.Call<AndroidJavaObject> ( "getSystemService", "vibrator" );
        _context = _currentActivity.Call<AndroidJavaObject> ( "getApplicationContext" );

        if ( AndroidVersion >= 26 ) {
            _vibrationEffect = new AndroidJavaClass ( "android.os.VibrationEffect" );
        }
    }

    ///<summary>
    /// Only on Android
    /// https://developer.android.com/reference/android/os/Vibrator.html#vibrate(long)
    ///</summary>
    private static void VibrateAndroid ( long milliseconds )
    {

        if ( Application.isMobilePlatform )
        {
            RunAsync(() =>
            {
                if (IsAndroidVersionMoreThan26)
                {
                    var createOneShot =
                        _vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, -1);
                    _vibrator.Call("vibrate", createOneShot);

                }
                else
                {
                    _vibrator.Call("vibrate", milliseconds);
                }
            });
        }
    }

    ///<summary>
    /// Only on Android
    /// https://proandroiddev.com/using-vibrate-in-android-b0e3ef5d5e07
    ///</summary>
    public static void VibrateAndroid ( long[] pattern, int repeat )
    {
        if ( Application.isMobilePlatform )
        {
            RunAsync(() =>
            {
                if (IsAndroidVersionMoreThan26)
                {
                    var createWaveform =
                        _vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                    _vibrator.Call("vibrate", createWaveform);

                }
                else
                {
                    _vibrator.Call("vibrate", pattern, repeat);
                }
            });
        }
    }

    private static int AndroidVersion {
        get {
            int iVersionNumber = 0;
            if (Application.platform != RuntimePlatform.Android) return iVersionNumber;
            
            var androidVersion = SystemInfo.operatingSystem;
            var sdkPos = androidVersion.IndexOf ( "API-" );
            iVersionNumber = int.Parse ( androidVersion.Substring ( sdkPos + 4, 2 ).ToString () );
            return iVersionNumber;
        }
    }

    private static void RunAsync(Action action)
    {
        Task.Run(() =>
        {
            AndroidJNI.AttachCurrentThread();
            action.Invoke();
            AndroidJNI.DetachCurrentThread();
        });
    }

    public static bool HasVibrator()
    {
        AndroidJavaClass contextClass = new AndroidJavaClass ( "android.content.Context" );
        string Context_VIBRATOR_SERVICE = contextClass.GetStatic<string> ( "VIBRATOR_SERVICE" );
        AndroidJavaObject systemService = _context.Call<AndroidJavaObject> ( "getSystemService", Context_VIBRATOR_SERVICE );
        if ( systemService.Call<bool> ( "hasVibrator" ) ) {
            return true;
        }

        return false;
    }
    
    public static void Cancel()
    {
        Assert.IsTrue(_initialized);
        if (!Application.isMobilePlatform) return;
        _vibrator.Call ( "cancel" );      
    }
}

#endif
