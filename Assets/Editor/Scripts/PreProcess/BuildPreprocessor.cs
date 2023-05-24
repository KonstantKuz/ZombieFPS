using Editor.Scripts.Config;

namespace Editor.Scripts.PreProcess
{
    public static class BuildPreprocessor
    {
        private const string PLATFORM_BUILD_DEFINE = "PLATFORM_BUILD";
        private const string DEBUG_CONSOLE_DEFINE = "DEBUG_CONSOLE_ENABLED";
        private const string FPS_MONITOR_DEFINE = "FPS_MONITOR_ENABLED";

        public static void SetDefines(bool platformBuild, bool debugConsoleEnabled, bool fpsMonitorEnabled = false)
        {
            DefineSymbolsUtil.SetDefine(PLATFORM_BUILD_DEFINE, platformBuild);
            DefineSymbolsUtil.SetDefine(DEBUG_CONSOLE_DEFINE, debugConsoleEnabled);
            DefineSymbolsUtil.SetDefine(FPS_MONITOR_DEFINE, fpsMonitorEnabled);
        }

        public static void BuildLoggerConfig(string loggerLevel)
        {
            ConfigPreprocessor.BuildLoggerConfig(loggerLevel);
        }

        public static void DownloadConfigs(string configsUrl)
        {
            ConfigDownloaderWindow.Download(configsUrl);
        }
    }
}