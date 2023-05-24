using System;
using Editor.Scripts.PreProcess;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Editor.Scripts
{
    public static class BuildUtil
    {
        [MenuItem("Build/Build Android and Run with Deep Profiling")]
        public static void BuildAndroidForDeepProfiling()
        {
            BuildAndroidForProfiling(true);
        }
        
        [MenuItem("Build/Build Android and Run with Profiling")]
        public static void BuildAndroidForProfiling()
        {
            BuildAndroidForProfiling(false);
        }

        public static void BuildAndroidForProfiling(bool deepProfiling)
        {
            SetDevelopmentBuild(true);
            var outputPath = "build/agentAlpha.apk";
            var buildOptions = BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AutoRunPlayer;
            if (deepProfiling) buildOptions |= BuildOptions.EnableDeepProfilingSupport;
            var options = new BuildPlayerOptions
            {
                scenes = Builder.GetSceneList(),
                locationPathName = outputPath,
                target = BuildTarget.Android,
                options = buildOptions
            };
            
            FileUtil.DeleteFileOrDirectory(outputPath);
            var report = BuildPipeline.BuildPlayer(options);
            Report(report.summary);
        }

        private static void Report(BuildSummary summary)
        {
            switch (summary.result)
            {
                case BuildResult.Succeeded:
                case BuildResult.Failed:
                case BuildResult.Cancelled:                    
                case BuildResult.Unknown:                    
                    SetDevelopmentBuild(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SetDevelopmentBuild(bool value)
        {
            PlayerSettings.Android.useCustomKeystore = !value;
            ConfigPreprocessor.BuildLoggerConfig(value ? "WARN" : "TRACE");
            BuildPreprocessor.SetDefines(value, value, value);
        }
    }
}
