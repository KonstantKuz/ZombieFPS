﻿using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.Scripts.PreProcess
{
    public static class Builder
    {

        [MenuItem("Build/Build Android")]        
        public static void BuildAndroid()
        {
            ApplyCommonParams();
            
            var buildAab = HasCmdLineKey("-buildAab");
            var outputFileName = GetCmdLineArgumentValue("-outputFileName") ?? "game";
            var outputPath = $"build/{outputFileName}.{(buildAab ? "aab" : "apk")}";
            var options = GetBuildOptions(BuildTarget.Android, outputPath);

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = GetCmdLineArgumentValue("-keyStorePassword");
            var androidSdkRoot = GetCmdLineArgumentValue("-androidSdkRoot");
            if (androidSdkRoot != null)
            {
                EditorPrefs.SetString("AndroidSdkRoot", androidSdkRoot);
            }

            if (HasCmdLineKey("-noUnityLogo"))
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }
            
            EditorUserBuildSettings.buildAppBundle = buildAab;
            EditorUserBuildSettings.androidCreateSymbolsZip = buildAab;

            FileUtil.DeleteFileOrDirectory(outputPath);
            var report = BuildPipeline.BuildPlayer(options);
            Report(report.summary);
        }

        private static void ApplyCommonParams()
        {
            BuildPreprocessor.SetDefines(true, HasCmdLineKey("-debugConsole"));

            if (HasCmdLineKey("-loggerLevel"))
            {
                BuildPreprocessor.BuildLoggerConfig(GetCmdLineArgumentValue("-loggerLevel"));
            }
            if (HasCmdLineKey("-configsUrl"))
            {
                BuildPreprocessor.DownloadConfigs(GetCmdLineArgumentValue("-configsUrl"));
            }
        }

        public static string[] GetSceneList() => EditorBuildSettings.scenes.Select(it => it.path).ToArray();

        [MenuItem("Build/Build Ios")]
        public static void BuildIos()
        {
            ApplyCommonParams();        
        
            var options = GetBuildOptions(BuildTarget.iOS, "build/xcode");

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            PlayerSettings.iOS.iOSManualProvisioningProfileType = HasCmdLineKey("-distribution")
                ? ProvisioningProfileType.Distribution
                : ProvisioningProfileType.Development;
            PlayerSettings.iOS.iOSManualProvisioningProfileID = GetCmdLineArgumentValue("-provisionProfileId");
            PlayerSettings.iOS.buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();

            EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;


            var report = BuildPipeline.BuildPlayer(options);
            Report(report.summary);
        }

        private static BuildPlayerOptions GetBuildOptions(BuildTarget buildTarget, string outputPath)
        {
            var buildOptions = BuildOptions.None;
            if (HasCmdLineKey("-profiling")) buildOptions |= BuildOptions.Development | BuildOptions.ConnectWithProfiler;
            if (HasCmdLineKey("-deepProfiling")) buildOptions |= BuildOptions.EnableDeepProfilingSupport;

            return new BuildPlayerOptions
            {
                scenes = GetSceneList(),
                locationPathName = outputPath,
                target = buildTarget,
                options = buildOptions
            };
        }

        private static void Report(BuildSummary summary)
        {
            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded");
                    break;
                case BuildResult.Failed:
                case BuildResult.Cancelled:                    
                case BuildResult.Unknown:                    
                    Debug.Log("Build failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (HasCmdLineKey("-quit"))
            {
                EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
            }
        }

        [CanBeNull]
        private static string GetCmdLineArgumentValue(string key)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == key)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static bool HasCmdLineKey(string key)
        {
            var args = Environment.GetCommandLineArgs();
            return args.Any(it => it == key);
        }
    }
}