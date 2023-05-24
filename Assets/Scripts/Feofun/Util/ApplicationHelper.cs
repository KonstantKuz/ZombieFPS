﻿using UnityEngine;

namespace Feofun.Util
{
    public static class ApplicationHelper
    {
        public static bool IsSimulator =>
                !UnityEngine.Device.Application.isEditor && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor
                                          || Application.platform == RuntimePlatform.LinuxEditor);
    }
}