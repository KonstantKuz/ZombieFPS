#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Editor.Scripts.PostProcess
{
    public class IronSourceXCodePostProcess
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS) return;
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            var rootDict = plist.root;
            
            SetNSAdvertisingAttributionReportEndpoint(rootDict);
            SetNSAppTransportSecurity(rootDict);
        
            File.WriteAllText(plistPath, plist.WriteToString());

            Debug.Log("IronSource, Info.plist updated with NSAdvertisingAttributionReportEndpoint ");  
            Debug.Log("IronSource, Info.plist updated with NSAppTransportSecurity ");

        }
        private static void SetNSAdvertisingAttributionReportEndpoint(PlistElementDict rootDict)
        {
            rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://postbacks-is.com");
        }
        private static void SetNSAppTransportSecurity(PlistElementDict rootDict)
        {
            var dict = rootDict.CreateDict("NSAppTransportSecurity");
            dict.SetBoolean("NSAllowsArbitraryLoads", true);
        }
    }
}
#endif