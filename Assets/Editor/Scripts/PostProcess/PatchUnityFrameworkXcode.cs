﻿#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Editor.Scripts.PostProcess
{
    public class PatchUnityFrameworkXcode
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            SetUnityFrameworkCodeSignToManual(pathToBuiltProject);
        }

        private static void SetUnityFrameworkCodeSignToManual(string pathToBuiltProject)
        {
            Debug.Log("Patching xcode project to set manual signing");
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);

            SetManualCodeSign(project);
            FixLibrarySearchPath(project);

            project.WriteToFile(projectPath);
            Debug.Log("Xcode project is patched");
        }

        private static void SetManualCodeSign(PBXProject project)
        {
            var unityFrameworkTarget = project.GetUnityFrameworkTargetGuid();
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGN_STYLE", "Manual");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_REQUIRED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_ALLOWED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE", "");
            project.SetBuildProperty(unityFrameworkTarget, "EXPANDED_CODE_SIGN_IDENTITY", "");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE_SPECIFIER", "");
            project.SetBuildProperty(unityFrameworkTarget, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

            var target = project.GetUnityMainTargetGuid();
            project.SetBuildProperty(target, "CODE_SIGN_STYLE", "Manual");
            project.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        }

        private static void FixLibrarySearchPath(PBXProject project)
        {
            var target = project.GetUnityMainTargetGuid();
            project.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(inherited)");
        }
    }
}
#endif