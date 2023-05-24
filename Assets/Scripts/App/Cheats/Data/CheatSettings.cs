using System.Runtime.Serialization;

namespace App.Cheats.Data
{
    [DataContract]
    public class CheatSettings
    {
        [DataMember]
        public bool ConsoleEnabled; 
        [DataMember]
        public bool FPSMonitorEnabled;
        [DataMember]
        public bool ABTestCheatEnabled;
    }
}