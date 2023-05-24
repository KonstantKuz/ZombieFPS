#if !UNITY_IOS && !UNITY_ANDROID 
public static partial class Vibration
{
    public static void InitPlatform()
    {
    }
    
    public static void Vibrate(long time)
    {
    }

    public static void Cancel()
    {
    }
}

#endif
